# Related Articles Components

Components for the "See Also" section at the bottom of an article. In `.athena` documents, the `## See Also` section is automatically rendered using these components.

---

## RelatedArticles

Renders `<div class="see-also">` with a "See also" heading followed by a CSS grid of cards. The grid fills available width with `minmax(180px, 1fr)` columns.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `ChildContent` | `RenderFragment` | Yes | `RelatedArticleLink` cards |

```razor
<RelatedArticles>
    <RelatedArticleLink Text="List of Aenarian Churches" Link="list-of-aenarian-churches" />
</RelatedArticles>
```

---

## RelatedArticleLink

A single card in the see-also grid. Renders `<div class="card">` containing a link styled as `.name` in large serif.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Text` | `string` | Yes | Display text |
| `Link` | `string` | Yes | Target slug or href |
