# Article Components

Components for the main article body area. Used together to build a structured, styled article card.

---

## ArticleContainer

Outer card wrapper for an article. Applies rounded border and light background.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `ChildContent` | `RenderFragment` | Yes | Content inside the card |

```razor
<ArticleContainer>
    <ArticleTitle Title="Aenar" />
    <ArticleContent>...</ArticleContent>
</ArticleContainer>
```

---

## ArticleContent

Prose wrapper with padding and muted text color. Place all readable body content here.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `ChildContent` | `RenderFragment` | Yes | Article body content |

---

## ArticleTitle

Renders the article heading bar with a large `<h3>`.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Title` | `string` | No | Heading text. Defaults to `"Title Unknown"` |

```razor
<ArticleTitle Title="Aenar" />
```

---

## ArticleLink

Inline hyperlink styled in blue. Use inside body text for cross-article links.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Href` | `string` | Yes | Link target (slug or URL) |
| `ChildContent` | `RenderFragment` | Yes | Link display text |

```razor
<ArticleLink Href="arethian">Arethian language</ArticleLink>
```
