# Article Components

Components for the main article body area. Used together to compose a structured article.

---

## ArticleContainer

Transparent wrapper — passes children straight through with no wrapping element. Exists for structural symmetry in the page template.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `ChildContent` | `RenderFragment` | Yes | Content inside the article |

```razor
<ArticleContainer>
    <ArticleTitle Title="Aenar" />
    <ArticleContent>...</ArticleContent>
</ArticleContainer>
```

---

## ArticleContent

Transparent wrapper — passes children straight through. Prose styling is applied by `codex.css` on the parent `<article>` element via `ColumnLeft`.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `ChildContent` | `RenderFragment` | Yes | Article body content |

---

## ArticleTitle

Renders the article heading as `<h1 class="article-title">`. Styled in large serif with a generous bottom margin.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Title` | `string` | No | Heading text. Defaults to `"Title Unknown"` |

```razor
<ArticleTitle Title="Aenar" />
```

---

## ArticleLink

Inline hyperlink. Uses the global `a` style from `codex.css` (muted underline that strengthens on hover). Use inside body text for cross-article links.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Href` | `string` | Yes | Link target (slug or URL) |
| `ChildContent` | `RenderFragment` | Yes | Link display text |

```razor
<ArticleLink Href="arethian">Arethian language</ArticleLink>
```
