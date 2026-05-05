# Language Components

Components for displaying multilingual names and IPA pronunciations, typically inside `InfoboxTitle`.

---

## IPAOption

Renders an inline span: `Language: Text [IPA]` with a link to the language article.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Language` | `string` | Yes | Language name (also used as the link href slug) |
| `Text` | `string` | Yes | Name in that language |
| `IPA` | `string` | Yes | IPA transcription string |

```razor
<IPAOption Language="Arethian" Text="εναρ" IPA="eːnɑr" />
```

Renders as: *Arethian: εναρ [eːnɑr]*

---

## LanguageOption

Renders an italicized name followed by the language name in parentheses. Used for compact name listings in `InfoboxTitle`.

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `Name` | `string` | Yes | Name text (rendered in italics) |
| `Language` | `string` | Yes | Language label (rendered in parentheses) |

```razor
<LanguageOption Name="εναρ" Language="Arethian" />
```

Renders as: *εναρ* (Arethian)
