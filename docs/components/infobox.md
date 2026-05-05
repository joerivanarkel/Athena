# Infobox Components

Components for the right-column summary card. Maps to the `[!infobox]` callout pattern in Obsidian.

---

## Infobox

Outer card container for the infobox.

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

Header section of the infobox. Shows the title and optional language name variants below it.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Title` | `string` | Yes | Primary title text |
| `ChildContent` | `RenderFragment` | No | Language options (`LanguageOption` components) |

---

## InfoboxDetails

Body section of the infobox. Renders an optional image and a detail table.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `ChildContent` | `RenderFragment` | Yes | Detail rows (`InfoboxDetailCell` components) |
| `Image` | `RenderFragment` | No | Image slot (renders above the detail table) |

---

## InfoboxDetailCell

A single key/value row inside `InfoboxDetails`.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Item` | `string` | Yes | Row label (left column) |
| `Value` | `string` | Yes | Row value (right column); supports inline HTML |
