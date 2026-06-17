from __future__ import annotations

import json
import subprocess
from datetime import datetime, timedelta, timezone
from pathlib import Path


blacklist = ["Collector", "Snecko", "SlimeBoss", "Gremlins", "Hermit"]
baselibversion = "3.2.1"

MODS_ROOT = Path(r"C:\Program Files (x86)\Steam\steamapps\common\Slay the Spire 2\mods")
DOWNFALL_DIR = MODS_ROOT / "Downfall"
DOWNFALL_U_DIR = MODS_ROOT / "DownfallU"
SEVEN_ZIP = Path(r"C:\Program Files\7-Zip\7z.exe")


def make_version() -> str:
    now = datetime.now(timezone(timedelta(hours=8)))
    return now.strftime("%Y.%m%d.%H%M%S")


def mod_name(path: Path) -> str:
    return path.stem if path.is_file() else path.name


def update_baselib_dependency(data: object) -> None:
    if not isinstance(data, dict):
        return

    dependencies = data.get("dependencies")
    if not isinstance(dependencies, list):
        return

    for dependency in dependencies:
        if not isinstance(dependency, dict):
            continue

        dependency_id = dependency.get("id")
        if isinstance(dependency_id, str) and dependency_id.lower() == "baselib":
            if "min_version" in dependency:
                dependency["min_version"] = baselibversion
            else:
                dependency["minversion"] = baselibversion


def update_json(path: Path, version: str) -> None:
    with path.open("r", encoding="utf-8-sig") as f:
        data = json.load(f)

    if isinstance(data, dict):
        data["version"] = version
        update_baselib_dependency(data)

    with path.open("w", encoding="utf-8", newline="\n") as f:
        json.dump(data, f, ensure_ascii=False, indent=2)
        f.write("\n")


def get_downfall_sources() -> list[Path]:
    if not DOWNFALL_DIR.is_dir():
        raise FileNotFoundError(f"Downfall directory not found: {DOWNFALL_DIR}")

    sources: list[Path] = []
    for path in DOWNFALL_DIR.iterdir():
        if mod_name(path) in blacklist:
            print(f"skipping {path.name}")
            continue
        sources.append(path)
    return sources


def update_downfall_jsons(version: str) -> None:
    if not DOWNFALL_DIR.is_dir():
        raise FileNotFoundError(f"Downfall directory not found: {DOWNFALL_DIR}")

    for path in DOWNFALL_DIR.iterdir():
        if path.is_file() and path.suffix.lower() == ".json":
            update_json(path, version)
            print(f"updated {path.name}")


def find_readme() -> Path:
    matches = sorted(
        path for path in MODS_ROOT.glob("读我*.txt") if path.is_file()
    )
    if len(matches) != 1:
        names = ", ".join(path.name for path in matches) or "<none>"
        raise RuntimeError(f"Expected exactly one 读我*.txt under {MODS_ROOT}, found: {names}")
    return matches[0]


def build_archive(readme: Path, version: str, downfall_sources: list[Path]) -> Path:
    if not SEVEN_ZIP.is_file():
        raise FileNotFoundError(f"7-Zip not found: {SEVEN_ZIP}")
    if not DOWNFALL_U_DIR.is_dir():
        raise FileNotFoundError(f"DownfallU directory not found: {DOWNFALL_U_DIR}")

    output_archive = MODS_ROOT / f"Downfall {version}.zip"
    if output_archive.exists():
        output_archive.unlink()

    command = [
        str(SEVEN_ZIP),
        "a",
        "-tzip",
        str(output_archive),
    ]
    for source in downfall_sources:
        command.append(str(source.relative_to(MODS_ROOT)))
    command.append(readme.name)
    command.append(r"DownfallU\*")

    subprocess.run(command, cwd=MODS_ROOT, check=True)
    return output_archive


def main() -> None:
    version = make_version()
    print(f"version {version}")

    update_downfall_jsons(version)
    downfall_sources = get_downfall_sources()
    readme = find_readme()
    output_archive = build_archive(readme, version, downfall_sources)

    print(f"created {output_archive}")


if __name__ == "__main__":
    main()
