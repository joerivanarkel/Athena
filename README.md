# Athena

A static site generator for worldbuilding encyclopedias. Parses `.athena` files (YAML frontmatter + Markdown body) and renders them as HTML using Blazor components.

## Architecture

| Project | Role |
|---------|------|
| `Athena.Content` | Parses `.athena` files into `AthenaDocument` records |
| `Athena.Presentation.Components` | Blazor component library — layout, article, infobox, TOC, etc. |
| `Athena.Ssg` | CLI that drives the render pipeline and writes static HTML output |

## Styling

Styles live in `src/Athena.Ssg/wwwroot/css/codex.css` — a single hand-authored CSS file with no build step required.

### Design system

The stylesheet uses CSS custom properties defined on `:root`:

| Variable | Purpose |
|----------|---------|
| `--paper` | Page background (warm off-white) |
| `--ink` / `--ink-2` / `--ink-3` | Text at three levels of emphasis |
| `--rule` / `--rule-2` | Divider lines |
| `--rubric` | Accent color (warm red) — numerals, drop-cap, hover states |
| `--link` | Hyperlink color (muted blue) |
| `--serif` | EB Garamond — body text, headings, drop-cap |
| `--sans` | Inter — UI labels, section headings, small caps |
| `--mono` | JetBrains Mono — TOC numerals, metadata |
| `--rail-w` | Right rail width (300 px, 260 px at mid breakpoint) |

Google Fonts are loaded in `HtmlShell.cs` — no local font files needed.

### Layout

Pages use a CSS grid: `minmax(0, 1fr) var(--rail-w)`. The article occupies the left column (`<article>` via `ColumnLeft`); the infobox and TOC occupy the right rail (`<aside class="rail">` via `ColumnRight`). The grid collapses to a single column below 860 px.

### Key CSS classes

| Class | Element | Description |
|-------|---------|-------------|
| `.content-wrap` | `<div>` | Grid container (max-width 1200 px) |
| `h1.article-title` | `<h1>` | Large serif title |
| `h2.section` | `<h2>` | Uppercase sans section heading with auto roman-numeral prefix |
| `h3.sub` | `<h3>` | Italic serif sub-heading |
| `.infobox` | `<div>` | Right-rail summary card |
| `.ib-name` / `.ib-alt` | `<div>` | Infobox title and alternate names |
| `.toc` | `<div>` | Table of contents with roman-numeral counters |
| `.see-also` | `<div>` | Card grid for related articles |
| `.refs` | `<div>` | Reference list with roman-numeral counters |
| `.index-wrap` | `<div>` | Index page container |

## Running the SSG

```bash
dotnet run --project src/Athena.Ssg
```

Output is written to `src/Athena.Ssg/bin/Debug/net9.0/output/`. Copy `wwwroot/css/codex.css` (and any asset files) alongside the HTML for a self-contained site.

## Content format

See [`docs/format/athena-format.md`](docs/format/athena-format.md) for the full `.athena` file specification.

## Component documentation

See the [`docs/components/`](docs/components/) directory for per-component API references.
