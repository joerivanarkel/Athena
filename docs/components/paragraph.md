# Paragraph Component

## Paragraph

A named content section. Renders an optional `<h2>` heading followed by body content. Maps to a single H1/H2 section in a `.athena` document body.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Title` | `string` | No | Section heading. Omit for a header-free block (e.g. the article intro) |
| `ChildContent` | `RenderFragment` | Yes | Section body content |

```razor
<Paragraph Title="Religion">
    The Aenarian religion takes place in large communions...
</Paragraph>

<Paragraph>
    Introductory paragraph with no heading.
</Paragraph>
```
