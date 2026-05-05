# Layout Components

Top-level layout primitives that establish the two-column grid used on article pages.

---

## TopContainer

Outer grid container. Renders `<div class="content-wrap">` — a CSS grid with a main content column and a fixed-width rail column. Collapses to a single column below 860 px.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `ChildContent` | `RenderFragment` | Yes | Expects `ColumnLeft` and `ColumnRight` |

```razor
<TopContainer>
    <ColumnLeft>...</ColumnLeft>
    <ColumnRight>...</ColumnRight>
</TopContainer>
```

---

## ColumnLeft

Main content column. Renders as `<article>` — takes the remaining grid width after the rail. Collapses to full width on narrow viewports.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `ChildContent` | `RenderFragment` | Yes | Article body content |

---

## ColumnRight

Rail column. Renders as `<aside class="rail">` — a flex column, 300 px wide (260 px on mid-size viewports). Hosts `Infobox` and `TableOfContents`.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `ChildContent` | `RenderFragment` | Yes | Rail content |
