# .athena Format Specification

The `.athena` format is a plain-text content format used by the Athena SSG to define articles. It is Athena-native — designed to map cleanly onto the Blazor component tree, not to be Obsidian-compatible.

A `.athena` file consists of two parts:

1. **YAML frontmatter** — structural metadata (title, infobox, tags)
2. **Markdown body** — article text with heading-based sections

---

## Frontmatter

The frontmatter is delimited by `---` at the top of the file. It is parsed by `AthenaParser` using YamlDotNet.

### Fields

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `title` | string | Yes | Article heading. Maps to `ArticleTitle.Title` |
| `slug` | string | No | URL path segment (e.g. `aenar`). Auto-generated from title if omitted |
| `category` | string | No | Content category (e.g. `deity`, `location`, `language`) |
| `tags` | string[] | No | Flat tag list for filtering |
| `infobox` | object | No | Right-column summary card. See [Infobox](#infobox) |

### Infobox

The `infobox` block maps directly to the `Infobox`, `InfoboxTitle`, `InfoboxDetails`, and `InfoboxDetailCell` components.

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `infobox.title` | string | No | Infobox heading. Defaults to `title` |
| `infobox.languages` | array | No | Language name variants. Each maps to a `LanguageOption` |
| `infobox.languages[].name` | string | Yes | Name in that language |
| `infobox.languages[].language` | string | Yes | Language label |
| `infobox.image.src` | string | No | Image path or URL |
| `infobox.image.alt` | string | No | Alt text |
| `infobox.details` | array | No | Key/value rows. Each maps to `InfoboxDetailCell` |
| `infobox.details[].item` | string | Yes | Row label |
| `infobox.details[].value` | string | Yes | Row value. Supports inline HTML (e.g. `<ul>`) |

### Example frontmatter

```yaml
---
title: Aenar
slug: aenar
category: deity
tags: [deity, fire, war, religion]

infobox:
  title: Aenar
  languages:
    - name: εναρ
      language: Arethian
  image:
    src: Aenar.jpg
    alt: Aenar
  details:
    - item: Gender
      value: Male
    - item: Holy Day
      value: "Blazing Dawn, 14th of Kuresohl"
---
```

---

## Body

The body is CommonMark Markdown. The parser processes it in three passes:

1. **Strip** `%%inline comments%%` (Obsidian translator notes)
2. **Extract** footnote definitions (`[^key]: text`) from the bottom
3. **Split** the remaining text into a section tree by heading level

### Sections

Headings define the section structure:

| Heading | Level | Maps to |
|---------|-------|---------|
| `# Title` | 1 | Top-level `<Paragraph Title="...">` |
| `## Title` | 2 | Child section; nested `<Paragraph>` or H2 within parent |
| `### Title` | 3 | Deeper nesting |
| *(no heading)* | 0 | Intro block rendered before the first section |

The intro block (text before the first `#` heading) is rendered as a header-free `<Paragraph>`.

### Reserved Section Names

| Section name | Rendered as |
|---|---|
| `## See Also` | `<RelatedArticles>` with `<RelatedArticleLink>` per list item |

### Footnotes

Footnote definitions at the end of the file are extracted by the parser and rendered as a `<ReferenceSection>`:

```markdown
[^herbad]: /'ɛɾbɑd'/
[^pf]: /'pɾɔpuɲɑtɔɾɛs fɑbulɑs'/
```

Inline footnote references (`[^herbad]`) remain in the body and are rendered as `<sup>` links by Markdig.

### Tables

Standard Markdown pipe tables are rendered as `<ArticleTable>` components. A caption line immediately before the table (Markdig table caption extension or custom pre-processing) maps to `ArticleTable.Title`.

```markdown
| Title | Equivalent |
|-------|------------|
| Dastur | Bishop |
| Herbad | Priest |
```

### Inline Annotations

| Syntax | Rendered as |
|--------|-------------|
| `[[slug]]` | `<ArticleLink Href="slug">slug</ArticleLink>` |
| `[[slug\|display text]]` | `<ArticleLink Href="slug">display text</ArticleLink>` |
| `/'IPA text'/` in a footnote | IPA transcription, displayed in the reference section |

Wikilinks are resolved by the SSG's `ContentService` — if the slug matches a known article the link is active; otherwise it renders as plain text.

### Obsidian Comment Stripping

`%%...%%` inline comments are stripped entirely by the parser. Use them for translator notes or content annotations that should not appear in the rendered output.

---

## Table of Contents

TOC entries are **auto-generated** from the section tree at render time. Level-1 and level-2 section names become `<HeaderLink>` entries in the `<TableOfContents>` sidebar component. There is no `toc:` field in the frontmatter.

---

## Full Annotated Example

```
---                                    ← frontmatter start
title: Aenar
slug: aenar
category: deity
tags: [deity, fire, war]

infobox:
  languages:
    - name: εναρ
      language: Arethian
  details:
    - item: Gender
      value: Male
---                                    ← frontmatter end

**Aenar** is one of the seven...       ← intro block (level 0, no heading)

# Religion                             ← level-1 section

The Aenarian religion...

## Sects                               ← level-2 child of Religion

- Sect of **Ignis**[^ignis]...

## See Also                            ← reserved: rendered as RelatedArticles

- [[List of Aenarian Churches]]

[^ignis]: /'iɲis'/                     ← footnote definition (extracted, rendered in ReferenceSection)
```

---

## Parser Behaviour Summary

| Input | Output |
|-------|--------|
| YAML between `---` delimiters | `AthenaMeta` record |
| `%%...%%` | Stripped |
| `[^key]: text` lines | `AthenaFootnote` list |
| Text before first `#` | `AthenaSection(Name="", Level=0, ...)` (intro) |
| `# Title` | `AthenaSection(Level=1)` with children |
| `## Title` | `AthenaSection(Level=2)`, child of nearest L1 |
| `### Title` | `AthenaSection(Level=3)`, child of nearest L2 |
