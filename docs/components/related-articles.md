# Related Articles Components

Components for the "See Also" bulleted list at the bottom of an article. In `.athena` documents, the `## See Also` section is automatically rendered using these components.

---

## RelatedArticles

Unordered list container for related article links.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `ChildContent` | `RenderFragment` | Yes | `RelatedArticleLink` items |

```razor
<RelatedArticles>
    <RelatedArticleLink Text="List of Aenarian Churches" Link="aenarian-churches" />
</RelatedArticles>
```

---

## RelatedArticleLink

A single bullet-point link to a related article.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Text` | `string` | Yes | Display text |
| `Link` | `string` | Yes | Target slug or href |
