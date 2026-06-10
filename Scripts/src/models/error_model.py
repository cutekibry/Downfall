from pathlib import Path
from typing import Literal

from pydantic import BaseModel
from rich.console import Console
from rich.text import Text


class ErrorModel(BaseModel):
    path: Path
    error_code: Literal["T1", "C1"]
    message: str

    def relative_path(self, root: Path | None = None) -> Path:
        root = (root or Path.cwd()).resolve()
        path = self.path.resolve()
        try:
            return path.relative_to(root)
        except ValueError:
            return path

    def print(self, root: Path | None = None, console: Console | None = None) -> None:
        console = console or Console()
        path = self.relative_path(root)

        line = Text()
        line.append("[", style="dim")
        line.append(self.error_code, style="bold red")
        line.append("] ", style="dim")
        line.append(str(path), style="cyan")
        line.append(": ", style="dim")
        line.append(self.message, style="white")
        console.print(line)
