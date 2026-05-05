# Reference Components

Components for the footnote/bibliography section at the bottom of an article. In `.athena` documents, footnotes (`[^key]: ...`) are automatically collected and rendered here.

---

## ReferenceSection

Ordered list container labeled "References". Wraps all `ReferenceLink` entries.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `ChildContent` | `RenderFragment` | Yes | `ReferenceLink` items |

```razor
<ReferenceSection>
    <ReferenceLink Href="https://example.com">Source title</ReferenceLink>
</ReferenceSection>
```

---

## ReferenceLink

A single numbered list item with a hyperlink. Maps to a footnote definition in `.athena`.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Href` | `string` | Yes | Link URL |
| `ChildContent` | `RenderFragment` | Yes | Citation text (e.g. IPA transcription or source title) |
