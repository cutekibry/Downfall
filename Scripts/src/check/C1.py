from pathlib import Path
import re
import sys

from rich.console import Console

from src.models.error_model import ErrorModel
from src.project_paths import resolve_repo_root


PROMPT_TOKEN = "SelectionScreenPrompt"
PROMPT_KEY_SUFFIX = ".selectionScreenPrompt"


def pascal_to_card_id(name: str) -> str:
    name = re.sub(r"(.)([A-Z][a-z]+)", r"\1_\2", name)
    name = re.sub(r"([a-z0-9])([A-Z])", r"\1_\2", name)
    return name.upper()


def collect_errors(root: Path | None = None) -> list[ErrorModel]:
    root = resolve_repo_root(root)
    errors: list[ErrorModel] = []

    for cards_json_path in sorted(root.glob("*/localization/eng/cards.json")):
        character_dir = cards_json_path.parts[-4]
        code_cards_dir = root / f"{character_dir}Code" / "Cards"
        if not code_cards_dir.is_dir():
            continue

        cards_json = cards_json_path.read_text(encoding="utf-8")
        key_prefix = character_dir.upper()

        for card_path in sorted(code_cards_dir.rglob("*.cs")):
            card_source = card_path.read_text(encoding="utf-8")
            if card_source.find(PROMPT_TOKEN) == -1:
                continue

            card_id = pascal_to_card_id(card_path.stem)
            expected_key = f"{key_prefix}-{card_id}{PROMPT_KEY_SUFFIX}"
            if cards_json.find(expected_key) == -1:
                try:
                    cards_json_display = cards_json_path.relative_to(root)
                except ValueError:
                    cards_json_display = cards_json_path
                errors.append(
                    ErrorModel(
                        path=card_path,
                        error_code="C1",
                        message=f"missing {expected_key} in {cards_json_display}",
                    )
                )

    return errors


def main() -> int:
    root = resolve_repo_root()
    errors = collect_errors(root)
    console = Console()

    if not errors:
        console.print("[green]OK[/green]: all SelectionScreenPrompt keys exist.")
        return 0

    console.print("[bold red]ERROR[/bold red]: missing selectionScreenPrompt localization keys:")
    for error in errors:
        error.print(root=root, console=console)
    return 1


if __name__ == "__main__":
    sys.exit(main())
