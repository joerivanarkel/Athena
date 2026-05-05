# Article Table Components

Components for rendering structured data tables inside article content. Maps to Markdown pipe tables in `.athena` documents.

---

## ArticleTable

Renders an HTML `<table>` with an optional title heading above it.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Title` | `string` | No | Caption shown above the table |
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

A `<th>` cell in the table header row.

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

A `<td>` data cell.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `ChildContent` | `RenderFragment` | Yes | Cell content |
