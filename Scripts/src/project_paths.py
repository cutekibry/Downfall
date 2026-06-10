from pathlib import Path


def resolve_repo_root(start: Path | None = None) -> Path:
    current = (start or Path.cwd()).resolve()

    for path in (current, *current.parents):
        if any(path.glob("*/localization/eng")):
            return path

    return current
