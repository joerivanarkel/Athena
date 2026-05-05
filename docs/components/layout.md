# Layout Components

Top-level responsive layout primitives. These establish the two-column grid used on article pages.

---

## TopContainer

Outer flex container. Stacks vertically on mobile, side-by-side (`md:flex-row`) on medium+ screens.

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

Main content column. Takes 3/4 width on `md+`, full width on mobile.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `ChildContent` | `RenderFragment` | Yes | Article body content |

---

## ColumnRight

Sidebar column. Takes 1/4 width on `md+`, full width on mobile. Hosts `Infobox` and `TableOfContents`.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `ChildContent` | `RenderFragment` | Yes | Sidebar content |
