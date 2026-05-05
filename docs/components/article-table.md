# Article Table Components

Components for rendering structured data tables inside article content. Maps to Markdown pipe tables in `.athena` documents.

In practice, Markdig renders pipe tables directly as `<table>` HTML inside the article. `codex.css` applies all table styles automatically — the Blazor components below are available for hand-authored Razor pages or edge cases where the Markdown table syntax is insufficient.

---

## ArticleTable

Renders an HTML `<table>` with an optional `<h3 class="sub">` title above it.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Title` | `string` | No | Caption shown above the table as a sub-heading |
| `TableHeaders` | `RenderFragment` | Yes | `<thead>` content via `TableHeader` components |
| `TableRows` | `RenderFragment` | Yes | `<tbody>` content via `TableRow`/`TableDrawer` components |

```razor
<ArticleTable Title="Priestly Order">
    <TableHeaders>
        <TableHeader>Title</TableHeader>
        <TableHeader>Equivalent</TableHeader>
    </TableHeaders>
    <TableRows>
        <TableRow>
            <TableDrawer>Dastur</TableDrawer>
            <TableDrawer>Bishop</TableDrawer>
        </TableRow>
    </TableRows>
</ArticleTable>
```

---

## TableHeader

A `<th>` cell in the table header row. Styled by `codex.css` as small-caps sans-serif with an ink-coloured bottom border.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `ChildContent` | `RenderFragment` | Yes | Header cell content |

---

## TableRow

A `<tr>` row in the table body. Contains `TableDrawer` cells.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `ChildContent` | `RenderFragment` | Yes | `TableDrawer` cells |

---

## TableDrawer

A `<td>` data cell. Styled with a rule-coloured bottom border; last row border is removed.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `ChildContent` | `RenderFragment` | Yes | Cell content |
