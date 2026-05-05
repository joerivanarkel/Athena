# Paragraph Component

## Paragraph

A content section with an optional heading. Maps to a single section in a `.athena` document body.

- `Level 1` (default) → renders `<h2 class="section">` with an auto-generated roman-numeral prefix via CSS counter
- `Level 2+` → renders `<h3 class="sub">` in italic serif
- No `Title` → passes `ChildContent` through with no wrapper (used for the intro block)

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Title` | `string` | No | Section heading. Omit for a header-free block (e.g. the article intro) |
| `Level` | `int` | No | Heading level: `1` for top-level sections, `2` for sub-sections, `3` for deeper nesting. Defaults to `1` |
| `ChildContent` | `RenderFragment` | No | Section body content |

The `id` attribute on the heading is automatically derived from `Title` (lowercased, spaces replaced with `-`) to enable TOC anchor links.

```razor
<Paragraph Title="Religion" Level="1">
    The Aenarian religion takes place in large communions...
    <Paragraph Title="Sects" Level="2">
        The religion divides into four principal sects.
    </Paragraph>
</Paragraph>

<Paragraph>
    Introductory paragraph with no heading.
</Paragraph>
```
