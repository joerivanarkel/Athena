# Infobox Components

Components for the right-rail summary card. Maps to the `infobox:` block in `.athena` frontmatter.

---

## Infobox

Outer container. Renders `<div class="infobox">` with a 2 px top border in ink color.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `ChildContent` | `RenderFragment` | Yes | Expects `InfoboxTitle` and `InfoboxDetails` |

```razor
<Infobox>
    <InfoboxTitle Title="Aenar">
        <LanguageOption Name="εναρ" Language="Arethian" />
    </InfoboxTitle>
    <InfoboxDetails>
        <ChildContent>
            <InfoboxDetailCell Item="Gender" Value="Male" />
        </ChildContent>
    </InfoboxDetails>
</Infobox>
```

---

## InfoboxTitle

Renders the entity name and optional alternate-language names.

- `Title` → `<div class="ib-name">` in large serif
- `ChildContent` → `<div class="ib-alt">` in italic, muted color (omitted when empty)

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Title` | `string` | Yes | Primary name |
| `ChildContent` | `RenderFragment` | No | Language name variants (`LanguageOption` components) |

---

## InfoboxDetails

Renders an optional image followed by a `<dl>` grid of key/value rows.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `ChildContent` | `RenderFragment` | Yes | `InfoboxDetailCell` rows |
| `Image` | `RenderFragment` | No | Image rendered above the `<dl>` |

---

## InfoboxDetailCell

A single row in the infobox `<dl>`. Renders `<dt>` (label) and `<dd>` (value) as adjacent grid cells.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Item` | `string` | Yes | Row label — rendered in small-caps sans-serif |
| `Value` | `string` | Yes | Row value; supports inline HTML (e.g. `<ul>` for multiple titles) |
