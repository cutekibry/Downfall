from pathlib import Path
from typing import Any
import json
import re
import sys

from rich.console import Console

from src.models.error_model import ErrorModel
from src.project_paths import resolve_repo_root


DYNAMIC_VAR_RE = re.compile(r"\{([a-zA-Z]+):([a-zA-Z]+)\(([0-9]*)\)\}")


def load_json(path: Path):
    return json.loads(path.read_text(encoding="utf-8-sig"))


def dynamic_vars(value: Any):
    if isinstance(value, str):
        text = value
    else:
        text = json.dumps(value, ensure_ascii=False, sort_keys=True)
    return set(DYNAMIC_VAR_RE.findall(text))


def format_vars(values) -> str:
    if not values:
        return "{}"
    return "{" + ", ".join(f"({a}, {b}, {c})" for a, b, c in sorted(values)) + "}"


def compare_json(eng_path: Path, lang_path: Path, errors: list[ErrorModel]) -> None:
    eng_data = load_json(eng_path)
    lang_data = load_json(lang_path)

    if not isinstance(eng_data, dict):
        errors.append(
            ErrorModel(
                path=eng_path,
                error_code="T1",
                message="eng json root is not an object",
            )
        )
        return
    if not isinstance(lang_data, dict):
        errors.append(
            ErrorModel(
                path=lang_path,
                error_code="T1",
                message="json root is not an object",
            )
        )
        return

    eng_keys = set(eng_data)
    lang_keys = set(lang_data)

    for key in sorted(eng_keys - lang_keys):
        errors.append(
            ErrorModel(path=lang_path, error_code="T1", message=f"missing key {key}")
        )
    for key in sorted(lang_keys - eng_keys):
        errors.append(
            ErrorModel(path=lang_path, error_code="T1", message=f"extra key {key}")
        )

    for key in sorted(eng_keys & lang_keys):
        eng_vars = dynamic_vars(eng_data[key])
        lang_vars = dynamic_vars(lang_data[key])
        if eng_vars != lang_vars:
            errors.append(
                ErrorModel(
                    path=lang_path,
                    error_code="T1",
                    message=(
                        f"key {key} dynamic vars differ: "
                        f"eng={format_vars(eng_vars)} lang={format_vars(lang_vars)}"
                    ),
                )
            )


def collect_errors(root: Path | None = None) -> list[ErrorModel]:
    root = resolve_repo_root(root)
    errors: list[ErrorModel] = []

    for localization_dir in sorted(root.glob("*/localization")):
        if not localization_dir.is_dir():
            continue

        eng_dir = localization_dir / "eng"
        if not eng_dir.is_dir():
            continue

        eng_files = {path.name: path for path in eng_dir.glob("*.json")}

        for lang_dir in sorted(path for path in localization_dir.iterdir() if path.is_dir()):
            if lang_dir.name == "eng":
                continue

            lang_files = {path.name: path for path in lang_dir.glob("*.json")}

            for file_name in sorted(set(eng_files) - set(lang_files)):
                errors.append(
                    ErrorModel(
                        path=lang_dir,
                        error_code="T1",
                        message=f"missing file {file_name}",
                    )
                )
            for file_name in sorted(set(lang_files) - set(eng_files)):
                errors.append(
                    ErrorModel(
                        path=lang_dir,
                        error_code="T1",
                        message=f"extra file {file_name}",
                    )
                )

            for file_name in sorted(set(eng_files) & set(lang_files)):
                compare_json(eng_files[file_name], lang_files[file_name], errors)

    return errors


def main() -> int:
    root = resolve_repo_root()
    errors = collect_errors(root)
    console = Console()

    if not errors:
        console.print("[green]OK[/green]: all localization files match eng keys and dynamic vars.")
        return 0

    console.print("[bold red]ERROR[/bold red]: localization files differ from eng:")
    for error in errors:
        error.print(root=root, console=console)
    return 1


if __name__ == "__main__":
    sys.exit(main())
