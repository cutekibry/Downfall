"""Export card data to CSV with Chinese localization.

Reads .cs card files (excluding Abstract/), extracts numeric values
from WithDamage/WithBlock/etc., looks up Chinese text from cards.json,
and outputs a CSV per character with columns: 卡牌名, 稀有度, 类型, 费用, 描述.
"""

import csv
import json
import re
import sys
from pathlib import Path


# ---------------------------------------------------------------------------
# Character configs
# ---------------------------------------------------------------------------
CHARACTERS = [
    {"name": "Guardian", "prefix": "GUARDIAN", "model": "GuardianCardModel"},
    {"name": "Awakened", "prefix": "AWAKENED", "model": "AwakenedCardModel"},
    {"name": "Hexaghost", "prefix": "HEXAGHOST", "model": "HexaghostCardModel"},
]

# ---------------------------------------------------------------------------
# Shared mappings
# ---------------------------------------------------------------------------
RARITY_MAP = {
    "Basic": "基础",
    "Common": "普通",
    "Uncommon": "罕见",
    "Rare": "稀有",
    "Ancient": "先古",
    "Token": "衍生",
    "None": "无",
}

RARITY_SORT = {
    "Basic": 0,
    "Common": 1,
    "Uncommon": 2,
    "Rare": 3,
    "Ancient": 4,
    "Token": 5,
    "None": 6,
}

TYPE_MAP = {
    "Attack": "攻击",
    "Skill": "技能",
    "Power": "能力",
}

TYPE_SORT = {
    "Attack": 0,
    "Skill": 1,
    "Power": 2,
}

KEYWORD_CN = {
    "Eternal": "永恒",
    "Ethereal": "虚无",
    "Exhaust": "消耗",
    "Innate": "固有",
    "Retain": "保留",
    "Sly": "奇巧",
    "Unplayable": "不能被打出",
    "Afterlife": "阴世",
    "Gem": "宝石",
    "Volatile": "消散",
    "Advance": "步进",
    "Retract": "回退",
}


# ---------------------------------------------------------------------------
# Helpers
# ---------------------------------------------------------------------------
def resolve_repo_root(start: Path | None = None) -> Path:
    current = (start or Path.cwd()).resolve()
    for path in (current, *current.parents):
        if any(path.glob("*/localization/eng")):
            return path
    return current


def class_name_to_key(class_name: str) -> str:
    s = re.sub(r"([a-z])([A-Z])", r"\1_\2", class_name)
    return s.upper()


# ---------------------------------------------------------------------------
# Parsing
# ---------------------------------------------------------------------------
def parse_constructor(content: str) -> dict | None:
    m = re.search(
        r":\s*base\s*\(\s*(\-?\d+)\s*,\s*CardType\.(\w+)\s*,\s*CardRarity\.(\w+)\s*,\s*TargetType\.\w+",
        content,
    )
    if not m:
        return None

    cost = int(m.group(1))
    card_type = m.group(2)
    rarity = m.group(3)

    body_start = m.end()
    brace_pos = content.find("{", body_start)
    if brace_pos == -1:
        return {"cost": cost, "type": card_type, "rarity": rarity, "values": {}}

    depth = 1
    end = brace_pos + 1
    while end < len(content) and depth > 0:
        if content[end] == "{":
            depth += 1
        elif content[end] == "}":
            depth -= 1
        end += 1
    body = content[brace_pos:end]

    values: dict[str, tuple[int, int | None]] = {}
    keywords: list[tuple[str, str | None]] = []

    def add_value(key: str, match: re.Match, index: int = 1):
        args = match.group(index)
        parts = [p.strip() for p in args.split(",")]
        nums = []
        for p in parts:
            try:
                nums.append(int(p))
            except ValueError:
                pass
        if nums:
            base = nums[0]
            upgrade = nums[1] if len(nums) >= 2 else None
            values[key] = (base, upgrade)

    # --- Numeric builders ---
    for pat, key in [
        (r"(?:this\.)?WithDamage\(([^)]+)\)", "Damage"),
        (r"(?:this\.)?WithBlock\(([^)]+)\)", "Block"),
        (r"(?:this\.)?WithRepeat\(([^)]+)\)", "Repeat"),
        (r"(?:this\.)?WithBrace\(([^)]+)\)", "Brace"),
        (r"(?:this\.)?WithPolish\(([^)]+)\)", "Polish"),
        (r"(?:this\.)?WithEnemyDamage\(([^)]+)\)", "EnemyDamage"),
        (r"(?:this\.)?WithAccelerate\(([^)]+)\)", "Accelerate"),
        (r"(?:this\.)?WithCards\(([^)]+)\)", "Cards"),
        (r"(?:this\.)?WithEnergy\(([^)]+)\)", "Energy"),
        (r"(?:this\.)?WithScry\(([^)]+)\)", "Scry"),
        (r"(?:this\.)?WithHpLoss\(([^)]+)\)", "HpLoss"),
    ]:
        for m in re.finditer(pat, body):
            add_value(key, m)

    # WithPower<T>(base, upgrade) or this.WithPower<T>(base, bool) or (base, upgrade, bool)
    for m in re.finditer(r"(?:this\.)?WithPower<(\w+)>\(([^)]+)\)", body):
        power_name = m.group(1)
        args = m.group(2)
        parts = [p.strip() for p in args.split(",")]
        nums = []
        for p in parts:
            try:
                nums.append(int(p))
            except ValueError:
                pass
        if nums:
            base = nums[0]
            if len(nums) >= 2 and parts[1].strip().lstrip("-").isdigit():
                upgrade = nums[1]
            elif len(nums) >= 3 and parts[2].strip().lstrip("-").isdigit():
                upgrade = nums[2]
            else:
                upgrade = None
            values[power_name] = (base, upgrade)

    # WithVar("name", base, upgrade) or WithVar("name", base)
    for m in re.finditer(r'(?:this\.)?WithVar\(["\'](\w+)["\']\s*,\s*([^)]+)\)', body):
        var_name = m.group(1)
        args = m.group(2)
        parts = [p.strip() for p in args.split(",")]
        nums = []
        for p in parts:
            try:
                nums.append(int(p))
            except ValueError:
                pass
        if nums:
            base = nums[0]
            upgrade = nums[1] if len(nums) >= 2 else None
            values[var_name] = (base, upgrade)

    # WithVars(new HpLossVar(2), new ScryVar(3, 1), ...)
    for m in re.finditer(r'new\s+(\w+)Var\(([^)]*)\)', body):
        var_type = m.group(1)  # "HpLoss"
        var_name = var_type
        args = m.group(2)
        parts = [p.strip() for p in args.split(",")]
        nums = []
        for p in parts:
            try:
                nums.append(int(p))
            except ValueError:
                pass
        if nums:
            base = nums[0]
            upgrade = nums[1] if len(nums) >= 2 else None
            if var_name not in values:
                values[var_name] = (base, upgrade)

    # WithCalculatedDamage/Block
    for m in re.finditer(r"(?:this\.)?WithCalculated(Damage|Block)\(([^)]+)\)", body):
        kind = m.group(1)
        args_str = m.group(2)
        parts = [p.strip() for p in args_str.split(",")]

        def try_int(s: str) -> int | None:
            # Strip named parameter prefixes like "bonusUpgrade: "
            s = re.sub(r"^\w+\s*:\s*", "", s.strip())
            try:
                return int(s)
            except ValueError:
                return None

        base = try_int(parts[0]) if len(parts) >= 1 else None
        if base is None:
            continue
        # 2-arg or 3-arg (base, calcFn [, bonusUpgrade: N]) — no position-1 upgrade
        if len(parts) <= 3 and try_int(parts[1]) is None:
            values["Calculated" + kind] = (base, None)
            if len(parts) >= 3:
                bonus = try_int(parts[2])
                if bonus is not None:
                    values["Extra" + kind] = (bonus, None)
            continue
        # 6-arg overload: (base, upgrade, calcFn, valueProp, extraBase, extraUpgrade)
        upgrade = try_int(parts[1])
        values["Calculated" + kind] = (base, upgrade)
        if len(parts) >= 6:
            extra_base = try_int(parts[4])
            extra_upgrade = try_int(parts[5]) if try_int(parts[5]) is not None else None
            if extra_base is not None:
                # When extraBase=0 (pure calc-damage card like PrismaticSpray),
                # the per-unit damage is in the upgrade slot (arg1).
                if extra_base == 0:
                    extra_base = upgrade or 1
                display_base = extra_base if extra_base > 0 else 1
                display_upgrade = extra_upgrade if extra_upgrade is not None else 0
                values["Extra" + kind] = (display_base, display_upgrade)
                if kind == "Block":
                    values["CalculationExtra"] = (display_base, display_upgrade)

    # WithCostUpgradeBy(delta)
    for m in re.finditer(r"WithCostUpgradeBy\(([^)]+)\)", body):
        try:
            delta = int(m.group(1).strip())
            values["CostUpgrade"] = (delta, None)
        except ValueError:
            pass

    # --- Keywords ---
    for m in re.finditer(r"WithKeyword\(CardKeyword\.(\w+)\)", body):
        keywords.append((m.group(1), None))
    for m in re.finditer(r"WithKeyword\(CardKeyword\.(\w+)\s*,\s*UpgradeType\.(\w+)\)", body):
        keywords.append((m.group(1), m.group(2)))
    # Character-specific keywords (GuardianKeyword, etc.)
    for m in re.finditer(r"WithKeyword\(\w*Keyword\.(\w+)\)", body):
        kw = m.group(1)
        if kw not in {e for e, _ in keywords}:  # avoid duplicating CardKeyword matches
            keywords.append((kw, None))
    for m in re.finditer(r"WithKeyword\(\w*Keyword\.(\w+)\s*,\s*UpgradeType\.(\w+)\)", body):
        kw = m.group(1)
        if kw not in {e for e, _ in keywords}:
            keywords.append((kw, m.group(2)))
    for m in re.finditer(r"WithKeywords\(([^)]+)\)", body):
        for kw_match in re.finditer(r"(?:Card|Hexaghost|Guardian)Keyword\.(\w+)", m.group(1)):
            keywords.append((kw_match.group(1), None))

    # this.WithAfterlife() — adds Ethereal + Afterlife (Hexaghost)
    if re.search(r"this\.WithAfterlife\(\)", body):
        keywords.append(("Ethereal", None))
        keywords.append(("Afterlife", None))

    return {"cost": cost, "type": card_type, "rarity": rarity, "values": values, "keywords": keywords}


# ---------------------------------------------------------------------------
# Description formatting
# ---------------------------------------------------------------------------
def format_description(template: str, values: dict[str, tuple[int, int | None]], cost: int) -> str:

    def split_at_top_pipe(inner: str) -> tuple[str, str, bool]:
        """Split inner text by '|' that is not inside nested {} braces.
        Returns (before, after, had_pipe)."""
        depth = 0
        for i, ch in enumerate(inner):
            if ch == "{":
                depth += 1
            elif ch == "}":
                depth -= 1
            elif ch == "|" and depth == 0:
                return inner[:i], inner[i+1:], True
        return inner, "", False

    def process_if_upgraded(text: str) -> str:
        prefix = "{IfUpgraded:show:"
        result = []
        i = 0
        plen = len(prefix)
        while i < len(text):
            if text[i : i + plen] == prefix:
                depth = 1
                j = i + plen
                while j < len(text) and depth > 0:
                    if text[j] == "{":
                        depth += 1
                    elif text[j] == "}":
                        depth -= 1
                    j += 1
                inner = text[i + plen : j - 1]
                a, b, had = split_at_top_pipe(inner)
                a = a.strip()
                b = b.strip()
                if had:
                    if a and b:
                        result.append(f"({b}|{a})")
                    elif b:
                        result.append(f"){b}(")
                    else:
                        result.append(f"({a})")
                else:
                    result.append(a)
                i = j
            else:
                result.append(text[i])
                i += 1
        return "".join(result)

    def process_in_combat(text: str) -> str:
        result = []
        i = 0
        while i < len(text):
            matched = False
            for prefix in ("{InCombat:cond:", "{InCombat:"):
                plen = len(prefix)
                if text[i : i + plen] == prefix:
                    depth = 1
                    j = i + plen
                    while j < len(text) and depth > 0:
                        if text[j] == "{":
                            depth += 1
                        elif text[j] == "}":
                            depth -= 1
                        j += 1
                    inner = text[i + plen : j - 1]
                    a, b, had = split_at_top_pipe(inner)
                    result.append(b if had else a)
                    i = j
                    matched = True
                    break
            if not matched:
                result.append(text[i])
                i += 1
        return "".join(result)

    result = process_if_upgraded(template)
    result = process_in_combat(result)

    def replace_token(match: re.Match) -> str:
        full = match.group(0)
        inner = match.group(1)

        if inner.startswith("energyPrefix:energyIcons("):
            n_match = re.search(r"energyIcons\((\d+)\)", inner)
            if n_match:
                n = int(n_match.group(1))
                return f"0[E]" if n == 0 else "[E]" * n
            return full

        if inner == "Energy:energyIcons()":
            v = values.get("Energy")
            if v is not None:
                base = v[0]
                if v[1] is not None and v[1] != 0:
                    return f"{'[E]' * base}([E])"
                return "[E]" * base
            return "[E]"

        # {Key:plural:X|Y} — X/Y may contain nested {tokens}
        m_pl = re.match(r"(\w+):plural:(.+)", inner)
        if m_pl:
            key = m_pl.group(1)
            rest = m_pl.group(2)
            # Find | at brace depth 0
            depth = 0
            pipe_pos = -1
            for idx, ch in enumerate(rest):
                if ch == "{":
                    depth += 1
                elif ch == "}":
                    depth -= 1
                elif ch == "|" and depth == 0:
                    pipe_pos = idx
                    break
            if pipe_pos >= 0:
                singular = rest[:pipe_pos]
                plural = rest[pipe_pos+1:]
                if key in values:
                    return singular if values[key][0] == 1 else plural
            return full

        m2 = re.match(r"(\w+):diff\(\)", inner)
        if m2:
            key = m2.group(1)
            if key in values:
                v = values[key]
                if v[1] is not None and v[1] != 0:
                    return f"{v[0]}({v[0] + v[1]})"
                return str(v[0])
            return full

        return full

    # Step 2a: handle {Key:plural:...} tokens first (they may contain nested {})
    def replace_nested(text: str) -> str:
        result = []
        i = 0
        while i < len(text):
            if text[i] == "{" and ":plural:" in text[i:i+50]:
                depth = 1
                j = i + 1
                while j < len(text) and depth > 0:
                    if text[j] == "{":
                        depth += 1
                    elif text[j] == "}":
                        depth -= 1
                    j += 1
                token = text[i:j]
                inner = token[1:-1]  # strip { }
                m_pl = re.match(r"(\w+):plural:(.+)", inner)
                if m_pl:
                    key = m_pl.group(1)
                    rest = m_pl.group(2)
                    depth2 = 0
                    pipe_pos = -1
                    for idx, ch in enumerate(rest):
                        if ch == "{":
                            depth2 += 1
                        elif ch == "}":
                            depth2 -= 1
                        elif ch == "|" and depth2 == 0:
                            pipe_pos = idx
                            break
                    if pipe_pos >= 0:
                        plural = rest[pipe_pos+1:]
                        result.append(plural)
                        i = j
                        continue
                result.append(token)
                i = j
            else:
                result.append(text[i])
                i += 1
        return "".join(result)

    result = replace_nested(result)
    result = re.sub(r"\{([^}]+)\}", replace_token, result)
    result = re.sub(r"\{([^}]+)\}", replace_token, result)

    result = re.sub(r"\[/?gold\]", "", result)
    result = result.replace("[afterlife]", "<").replace("[/afterlife]", ">")
    result = re.sub(r"\[color=[^\]]*\]", "", result)
    result = re.sub(r"\[/color\]", "", result)
    result = result.replace("\n", "")
    return result


# ---------------------------------------------------------------------------
# File discovery & card parsing
# ---------------------------------------------------------------------------
def find_card_files(root: Path, char_name: str) -> list[Path]:
    cards_dir = root / f"{char_name}Code" / "Cards"
    if not cards_dir.exists():
        print(f"  WARNING: Cards directory not found: {cards_dir}", file=sys.stderr)
        return []
    files = []
    for cs_file in cards_dir.rglob("*.cs"):
        if "Abstract" in cs_file.parts:
            continue
        files.append(cs_file)
    return files


def parse_card_file(filepath: Path, model_class: str) -> list[dict]:
    content = filepath.read_text(encoding="utf-8")
    content = re.sub(r"/\*.*?\*/", "", content, flags=re.DOTALL)
    content = re.sub(r"//[^\n]*", "", content)

    cards = []
    for m in re.finditer(r"public\s+class\s+(\w+)\s*:\s*([^{]+)", content):
        class_name = m.group(1)
        bases = m.group(2).strip()

        prefix = content[: m.start()]
        last_nl = prefix.rfind("\n")
        line_before = prefix[last_nl:m.start()].strip() if last_nl != -1 else prefix[:m.start()].strip()
        if "abstract" in line_before.lower():
            continue

        body_start = m.end()
        brace = content.find("{", body_start)
        if brace == -1:
            continue

        depth = 1
        end = brace + 1
        while end < len(content) and depth > 0:
            if content[end] == "{":
                depth += 1
            elif content[end] == "}":
                depth -= 1
            end += 1

        class_region = content[m.start() : end]

        if model_class not in bases:
            continue

        info = parse_constructor(class_region[brace - m.start():end - m.start()])
        if info is None:
            info = parse_constructor(class_region)

        if info:
            info["class_name"] = class_name
            info["chantable"] = "IChantable" in bases
            # X-cost cards: HasEnergyCostX => true
            if re.search(r"HasEnergyCostX\s*=>\s*true", class_region):
                info["cost"] = "X"
            # GemSlots (Guardian-specific, harmless for others)
            gm = re.search(r"public\s+int\s+GemSlots\s*=>\s*(.+?);", class_region)
            if gm:
                expr = gm.group(1).strip()
                if "IsUpgraded" in expr and "?" in expr:
                    m2 = re.search(r"\?\s*(\d+)\s*:\s*(\d+)", expr)
                    if m2:
                        info["gem_slots"] = (int(m2.group(2)), int(m2.group(1)))
                elif "+" in expr and "CurrentUpgradeLevel" in expr:
                    m2 = re.search(r"(\d+)\s*\+", expr)
                    if m2:
                        info["gem_slots"] = (int(m2.group(1)), None)
                else:
                    m2 = re.search(r"(\d+)", expr)
                    if m2:
                        info["gem_slots"] = (int(m2.group(1)), None)
            cards.append(info)

    return cards


# ---------------------------------------------------------------------------
# Per-character export
# ---------------------------------------------------------------------------
def export_character(root: Path, cfg: dict) -> tuple[str, int]:
    char_name = cfg["name"]
    prefix = cfg["prefix"]
    model = cfg["model"]

    loc_path = root / char_name / "localization" / "zhs" / "cards.json"
    if not loc_path.exists():
        print(f"  SKIP: no localization at {loc_path}")
        return char_name, 0
    with open(loc_path, "r", encoding="utf-8") as f:
        loc_data = json.load(f)

    # Load chants (Awakened-specific)
    chants_path = root / char_name / "localization" / "zhs" / "chants.json"
    chant_data = {}
    if chants_path.exists():
        with open(chants_path, "r", encoding="utf-8") as f:
            chant_data = json.load(f)

    card_files = find_card_files(root, char_name)
    if not card_files:
        print(f"  SKIP: no card files found")
        return char_name, 0

    all_cards = []
    for fpath in sorted(card_files):
        cards = parse_card_file(fpath, model)
        all_cards.extend(cards)

    rows = []
    for card in sorted(all_cards, key=lambda c: (
        RARITY_SORT.get(c.get("rarity", "None"), 99),
        TYPE_SORT.get(c.get("type", ""), 99),
        c["class_name"]
    )):
        class_name = card["class_name"]
        key = class_name_to_key(class_name)
        loc_key = f"{prefix}-{key}"
        vals = card.get("values", {})

        title = loc_data.get(f"{loc_key}.title", class_name)
        rarity_en = card.get("rarity", "None")
        rarity_cn = RARITY_MAP.get(rarity_en, rarity_en)
        type_en = card.get("type", "")
        type_cn = TYPE_MAP.get(type_en, type_en)

        cost = card.get("cost", "")
        cost_upgrade = vals.get("CostUpgrade")
        cost_str = f"{cost}({cost + cost_upgrade[0]})" if cost_upgrade else str(cost)

        # Keywords
        prefix_kws = []
        suffix_kws = []
        for kw_name, upgrade_type in card.get("keywords", []):
            cn = KEYWORD_CN.get(kw_name, kw_name)
            if upgrade_type == "Add":
                kw_str = f"({cn}。) "
            elif upgrade_type == "Remove":
                kw_str = f"){cn}。( "
            else:
                kw_str = f"{cn}。 "
            if kw_name == "Exhaust":
                suffix_kws.append(kw_str.rstrip())
            else:
                prefix_kws.append(kw_str.rstrip())

        desc_template = loc_data.get(f"{loc_key}.description", "")
        desc = format_description(desc_template, vals, cost) if desc_template else ""

        desc_parts = []
        if prefix_kws:
            desc_parts.extend(prefix_kws)
        if desc:
            desc_parts.append(desc)
        gem = card.get("gem_slots")
        if gem is not None:
            base, upgraded = gem
            desc_parts.append(f"[槽位{base}({upgraded})]" if upgraded is not None else f"[槽位{base}]")

        # Chant (Awakened)
        if card.get("chantable"):
            chant_text = chant_data.get(f"{loc_key}.chant", "")
            if chant_text:
                chant_text = format_description(chant_text, vals, cost)
                if not chant_text.endswith("。"):
                    chant_text += "。"
                desc_parts.append(f"吟唱：{chant_text}")

        if suffix_kws:
            desc_parts.extend(suffix_kws)
        desc = "".join(desc_parts)

        rows.append({
            "卡牌名": title,
            "稀有度": rarity_cn,
            "类型": type_cn,
            "费用": cost_str,
            "描述": desc,
        })

    out_dir = root / "scripts" / "output"
    out_dir.mkdir(parents=True, exist_ok=True)
    out_path = out_dir / f"{char_name.lower()}.csv"
    with open(out_path, "w", encoding="utf-8-sig", newline="") as f:
        writer = csv.DictWriter(f, fieldnames=["卡牌名", "稀有度", "类型", "费用", "描述"])
        writer.writeheader()
        writer.writerows(rows)

    return char_name, len(rows)


# ---------------------------------------------------------------------------
# Main
# ---------------------------------------------------------------------------
def main():
    root = resolve_repo_root()
    print(f"Repo root: {root}")

    for cfg in CHARACTERS:
        name, count = export_character(root, cfg)
        print(f"  {name}: {count} cards")


if __name__ == "__main__":
    main()
