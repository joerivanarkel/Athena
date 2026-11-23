#!/usr/bin/env python3
"""Convert the Obsidian 'Worldbuilding (Iagoband)' vault articles to .athena files.

Usage: python tools/import_obsidian.py <vault-dir> <athena-content-dir> <assets-images-dir>
Existing .athena files (matched by slug) are never overwritten.
Wikilinks to articles that are not converted are rendered as plain text.
"""
import re, sys, shutil, json
from pathlib import Path

vault, out_root, img_out = map(Path, sys.argv[1:4])
articles = vault / "Articles"
images_root = vault / "Images"
ZW = dict.fromkeys(map(ord, "​‌‍﻿"))


def clean(s):
    return s.translate(ZW)


def slugify(s):
    s = clean(s).replace("+", "").lower().replace("&", "and").replace("'", "").replace("’", "")
    return re.sub(r"[^a-z0-9]+", "-", s).strip("-")


def yq(s):  # YAML double-quoted scalar
    return json.dumps(s, ensure_ascii=False)


def split_fm(text):
    m = re.match(r"\A---\r?\n(.*?)\r?\n---\r?\n?", text, re.S)
    return (m.group(1), text[m.end():]) if m else ("", text)


def fm_tags(fm):
    return [t.lower() for t in re.findall(r"Worldbuilding/([A-Za-z-]+)", fm)]


# ---- pass 1: inventory ----------------------------------------------------
docs = []
for f in sorted(articles.rglob("*.md")):
    raw = f.read_text(encoding="utf-8-sig")
    fm, body = split_fm(raw)
    title = clean(f.stem).rstrip("+").strip()
    top = f.relative_to(articles).parts[0]
    cat = slugify(re.sub(r"^\d+\.\s*", "", top))
    docs.append(dict(path=f, fm=fm, body=body, tags=fm_tags(fm), title=title, slug=slugify(title), cat=cat))

by_slug = {}
for d in docs:
    by_slug.setdefault(d["slug"], []).append(d)
dups = {s: [x["path"].relative_to(articles).as_posix() for x in v] for s, v in by_slug.items() if len(v) > 1}
if dups:
    print("WARNING duplicate slugs:", dups)


def has_content(d):
    b = re.sub(r"^>.*$", "", d["body"], flags=re.M).strip()
    return bool(b) or "[!infobox" in d["body"]


skipped, keep = [], []
for d in docs:
    if "empty" in d["tags"] or not has_content(d):
        skipped.append(d["path"].relative_to(articles).as_posix())
    else:
        keep.append(d)
slugs = {d["slug"] for d in keep}
known = {clean(d["title"]).lower(): d["slug"] for d in docs}

# ---- images ---------------------------------------------------------------
img_index = {p.name.lower(): p for p in images_root.rglob("*") if p.is_file()}
copied, missing_imgs = set(), set()


def image_url(name):
    name = clean(name).split("|")[0].replace("\\", "").strip()
    p = img_index.get(name.lower())
    if not p:
        missing_imgs.add(name)
        return None
    dest = re.sub(r"[^A-Za-z0-9._-]+", "-", p.name)
    if dest not in copied:
        img_out.mkdir(parents=True, exist_ok=True)
        shutil.copy2(p, img_out / dest)
        copied.add(dest)
    return "assets/images/" + dest


# ---- links ----------------------------------------------------------------
def resolve(target):
    t = clean(target).split("#")[0].strip().replace("\\", "")
    s = known.get(t.lower().rstrip("+").strip())
    return s if s in slugs else None


def convert_links(text, in_table=False):
    def wl(m):
        target, alias = m.group(1), m.group(2)
        slug = resolve(target)
        disp = clean(alias if alias else target).split("#")[0].strip().replace("\\", "")
        if not alias:
            disp = disp.rstrip("+")
        if not slug:
            return disp
        if in_table:
            return f'<a href="{slug}">{disp}</a>'
        return f"[[{slug}|{disp}]]"

    return re.sub(r"\[\[([^\]|\\]+?)(?:\\?\|([^\]]+?))?\]\]", wl, text)


# ---- infobox --------------------------------------------------------------
def strip_md(s):
    return re.sub(r"\*\*(.+?)\*\*", r"\1", s).strip()


def parse_infobox(lines):
    ib = dict(title=None, native=None, image=None, details=[])
    for ln in lines:
        s = re.sub(r"^>\s?", "", ln).strip()
        if not s or re.match(r"^\[!infobox", s):
            continue
        m = re.match(r"^(#{1,6})\s*(.+)$", s)
        if m:
            h, lvl = clean(strip_md(m.group(2))), len(m.group(1))
            if lvl <= 3 and not ib["details"] and not ib["image"]:
                if ib["title"] is None:
                    ib["title"] = h
                elif ib["native"] is None:
                    ib["native"] = h
            continue
        m = re.match(r"^!\[\[([^\]]+)\]\]$", s)
        if m:
            if not ib["image"]:
                ib["image"] = image_url(m.group(1))
            continue
        if re.match(r"^[\s|:-]*$", s):
            continue
        if "|" in s:
            k, v = s.split("|", 1)
            k = strip_md(clean(k)).rstrip(":").strip()
            v = clean(v).strip().strip("|").strip()
            if k.lower() == "type" and v.lower() in ("name", ""):
                continue
            v = re.sub(
                r"!\[\[([^\]|\\]+)[^\]]*\]\]",
                lambda mm: (lambda u: f'<img src="{u}" height="25" alt="">' if u else "")(image_url(mm.group(1))),
                v,
            )
            v = convert_links(v, in_table=True)
            v = re.sub(r"#Worldbuilding/\S+", "", v).replace("<br>", "<br/>").strip()
            if k and v:
                ib["details"].append((k, v))
    return ib


def extract_infobox(body):
    lines, out, ib_lines, i = body.split("\n"), [], None, 0
    while i < len(lines):
        if ib_lines is None and re.match(r"^>\s*\[!infobox", lines[i]):
            ib_lines = []
            while i < len(lines) and lines[i].strip():
                ib_lines.append(lines[i])
                i += 1
            continue
        out.append(lines[i])
        i += 1
    return ib_lines, "\n".join(out)


# ---- body -----------------------------------------------------------------
def embed(name):
    """![[file.png]] -> markdown image; ![[Some Note]] (transclusion) -> link to that article."""
    if re.search(r"\.(png|jpe?g|gif|svg|webp)(\||$)", name, re.I):
        u = image_url(name)
        return f"![]({u})" if u else ""
    slug = resolve(name)
    return f"- [[{slug}|{clean(name).strip()}]]" if slug else ""


def convert_body(body):
    body = clean(body)
    body = re.sub(r"```(toc|leaflet|mermaid|dataview)[\s\S]*?```", "", body)
    body = re.sub(r"#Worldbuilding/\S+", "", body)
    out = []
    for ln in body.split("\n"):
        is_table = ln.count("|") >= 2
        ln = re.sub(
            r"!\[\[([^\]]+)\]\]",
            lambda m: embed(m.group(1)),
            ln,
        )
        if re.match(r"^#{1,6}\s", ln):
            # headings feed the TOC/anchors: plain text only
            ln = re.sub(r"\[\[([^\]|]+?)(?:\|([^\]]+?))?\]\]", lambda m: (m.group(2) or m.group(1)).strip(), ln)
            ln = ln.replace("**", "")
            out.append(ln)
            continue
        out.append(convert_links(ln, in_table=is_table))
    return re.sub(r"\n{3,}", "\n\n", "\n".join(out)).strip()


# ---- emit -----------------------------------------------------------------
existing = {p.stem for p in out_root.rglob("*.athena") if "generated: obsidian-import" not in p.read_text(encoding="utf-8")}
written, skipped_existing = [], []
for d in keep:
    if d["slug"] in existing:
        skipped_existing.append(d["slug"])
        continue
    ib_lines, rest = extract_infobox(d["body"])
    ib = parse_infobox(ib_lines) if ib_lines else None
    body = convert_body(rest)
    tags = sorted({d["cat"], *[t for t in d["tags"] if t not in ("infobox", "empty")]})
    fm = ["---", f"title: {yq(d['title'])}", f"slug: {d['slug']}", f"category: {d['cat']}", "generated: obsidian-import",
          "tags: [" + ", ".join(tags) + "]"]
    if ib:
        fm += ["", "infobox:", f"  title: {yq(ib['title'] or d['title'])}"]
        if ib["native"]:
            fm += ["  languages:", f"    - name: {yq(ib['native'])}", "      language: Native"]
        if ib["image"]:
            fm += ["  image:", f"    src: {yq(ib['image'])}", f"    alt: {yq(d['title'])}"]
        if ib["details"]:
            fm.append("  details:")
            for k, v in ib["details"]:
                fm += [f"    - item: {yq(k)}", f"      value: {yq(v)}"]
    fm.append("---")
    dest = out_root / d["cat"] / f"{d['slug']}.athena"
    dest.parent.mkdir(parents=True, exist_ok=True)
    dest.write_text("\n".join(fm) + "\n\n" + body + "\n", encoding="utf-8", newline="\n")
    written.append(d["slug"])

print(f"source notes: {len(docs)}  converted: {len(written)}  skipped empty: {len(skipped)}  "
      f"already existed: {skipped_existing}")
print(f"images copied: {len(copied)}  missing images: {sorted(missing_imgs)}")
print("skipped (empty):", *skipped, sep="\n  ")
