from pathlib import Path
import sys

from rich.console import Console

from src.check.C1 import collect_errors as collect_c1_errors
from src.check.T1 import collect_errors as collect_t1_errors
from src.project_paths import resolve_repo_root


def main() -> int:
    root = resolve_repo_root()
    console = Console()
    errors = [
        *collect_t1_errors(root),
        *collect_c1_errors(root),
    ]

    if not errors:
        console.print("[green]OK[/green]: all checks passed.")
        return 0

    console.print("[bold red]ERROR[/bold red]: checks failed:")
    for error in errors:
        error.print(root=root, console=console)
    return 1


if __name__ == "__main__":
    sys.exit(main())
