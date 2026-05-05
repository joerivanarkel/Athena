# Table of Contents Components

Components for the right-column TOC card. In the SSG, TOC entries are auto-generated from document headings.

---

## TableOfContents

Card container with a bulleted list of in-page anchor links.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `ChildContent` | `RenderFragment` | Yes | `HeaderLink` entries |

```razor
<TableOfContents>
    <HeaderLink Value="Religion" Href="religion" />
    <HeaderLink Value="Cultural" Href="cultural" />
</TableOfContents>
```

---

## HeaderLink

A single anchor list item in the TOC. Uses `NavigationManager` to build the full href from the base URI.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Value` | `string` | Yes | Display text |
| `Href` | `string` | Yes | Anchor id (without `#`) |

> **Note:** In the SSG context, `NavigationManager` is provided by `SsgNavigationManager` which returns the configured base href. The computed link becomes `{baseHref}#{Href}`.
