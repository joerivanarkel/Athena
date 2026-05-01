# SSG Implementation Plan — Athena

## Overview

This document outlines a plan to implement Static Site Generation (SSG) for the Athena
Blazor wiki application. The goal is to pre-render article pages to static HTML at build
time, reducing initial load times and improving SEO without abandoning the existing Blazor
component model.

---

## Current Architecture

| Aspect | Current State |
|---|---|
| SDK | `Microsoft.NET.Sdk.BlazorWebAssembly` (.NET 7) |
| Rendering | Client-Side Rendering (CSR) via WebAssembly |
| Content storage | Hard-coded markup inside individual `.razor` page files |
| Routing | One `.razor` page per article (e.g. `Aenar.razor` → `/Aenar`) |

**Pain points:**
- Each new article requires a new `.razor` file and a code deploy.
- Users must download the WASM runtime before any content is visible.
- Search engines cannot index content without JavaScript execution.

---

## Goals

1. Pre-render every article page to static HTML at build time.
2. Separate content (data) from presentation (components).
3. Keep the existing reusable component library (`Presentation.Components`) intact.
4. Deploy the finished output as a static site (e.g. GitHub Pages or Azure Static Web Apps).

---

## Recommended Approach: Upgrade to .NET 8 Blazor with Static Rendering

.NET 8 introduced first-class support for Blazor Static Server-Side Rendering (SSR) and
static output. Pages decorated with `@attribute [StreamRendering(false)]` (or simply no
interactive render mode) are rendered to HTML on the server and shipped as plain HTML — no
WASM download required.

### High-level steps

```
Blazor WASM (.NET 7, CSR)
        │
        ▼
Blazor Web App (.NET 8, Static SSR + optional WASM islands)
        │
        ▼
Content data layer (JSON files → ArticleService)
        │
        ▼
Single dynamic ArticlePage.razor reads data and renders components
        │
        ▼
dotnet publish → static HTML output → deploy to Pages / CDN
```

---

## Implementation Phases

### Phase 1 — Upgrade to .NET 8

- Update both `.csproj` files from `net7.0` to `net8.0`.
- Change the `Presentation` project SDK from `Microsoft.NET.Sdk.BlazorWebAssembly` to
  `Microsoft.NET.Sdk.Web`.
- Replace `Program.cs` with the .NET 8 `WebApplication` host builder pattern.
- Remove `Microsoft.AspNetCore.Components.WebAssembly` packages; add
  `Microsoft.AspNetCore.Components.Web` and configure the Blazor static render mode.
- Verify the existing wrapper components still compile under the new SDK.

### Phase 2 — Content Data Layer (`Athena.Content`)

Create a new class library project that holds article data separate from the UI.

**Directory layout:**

```
Athena.Content/
  Models/
    Article.cs         # root article model
    ArticleSection.cs  # titled paragraph with optional sub-sections
    ArticleTable.cs    # table model (headers + rows)
    Infobox.cs         # infobox metadata
    InfoboxDetail.cs   # single key/value row
    RelatedLink.cs     # related-articles link
  Services/
    IArticleService.cs
    JsonArticleService.cs   # reads from /wwwroot/content/*.json
  Content/               # (or wwwroot/content/)
    aenar.json
    phoenix.json
    ...
```

**Sample `Article` model:**

```csharp
public record Article(
    string Id,
    string Title,
    IReadOnlyList<LanguageAlternative> LanguageNames,
    IReadOnlyList<ArticleSection> Sections,
    Infobox? Infobox,
    IReadOnlyList<RelatedLink> RelatedArticles
);
```

**Sample JSON file (`aenar.json`):**

```json
{
  "id": "aenar",
  "title": "Aenar",
  "languageNames": [{ "language": "Arethian", "name": "εναρ", "ipa": "eːnɑr" }],
  "sections": [
    {
      "title": null,
      "body": "**Aenar** … is one of the seven elemental deities …"
    },
    {
      "title": "Religion",
      "body": "The Aenarian religion takes place …"
    }
  ],
  "infobox": {
    "details": [
      { "item": "Divine Classification", "value": "Religions (Elemental)" }
    ]
  },
  "relatedArticles": [
    { "text": "List of Aenarian Churches", "link": "" }
  ]
}
```

### Phase 3 — Dynamic Article Page

Replace the per-article `.razor` files with a single data-driven page.

```razor
@page "/article/{ArticleId}"
@inject IArticleService ArticleService

@if (_article is null)
{
    <p>Article not found.</p>
}
else
{
    <TopContainer>
        <ColumnLeft>
            <ArticleContainer>
                <ArticleTitle title="@_article.Title" />
                <ArticleContent>
                    @foreach (var section in _article.Sections)
                    {
                        <Paragraph Title="@section.Title">@section.Body</Paragraph>
                    }
                </ArticleContent>
            </ArticleContainer>
        </ColumnLeft>
        <ColumnRight>
            @if (_article.Infobox is not null)
            {
                <!-- render infobox -->
            }
            <TableOfContents>
                @foreach (var section in _article.Sections.Where(s => s.Title is not null))
                {
                    <HeaderLink Value="@section.Title" Href="@ToAnchor(section.Title!)" />
                }
            </TableOfContents>
        </ColumnRight>
    </TopContainer>
}

@code {
    [Parameter] public string ArticleId { get; set; } = "";
    private Article? _article;

    protected override async Task OnParametersSetAsync()
        => _article = await ArticleService.GetAsync(ArticleId);

    private static string ToAnchor(string title)
        => title.ToLowerInvariant().Replace(' ', '-');
}
```

### Phase 4 — Static Output & Deployment

**Build-time pre-rendering with .NET 8:**

Add the following to `Presentation.csproj`:

```xml
<PropertyGroup>
  <BlazorEnableTimeZoneSupport>false</BlazorEnableTimeZoneSupport>
  <PublishSingleFile>false</PublishSingleFile>
  <!-- Enable static web asset generation -->
  <StaticWebAssetBasePath>/</StaticWebAssetBasePath>
</PropertyGroup>
```

Run:

```bash
dotnet publish -c Release -o ./publish
```

The output directory (`publish/wwwroot`) contains fully rendered HTML files that can be
served by any static file host.

**GitHub Actions workflow (`.github/workflows/deploy.yml`):**

```yaml
name: Build and Deploy

on:
  push:
    branches: [master]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: 8.x
      - run: npm ci
      - run: dotnet publish Presentation/Presentation.csproj -c Release -o ./publish
      - uses: actions/upload-pages-artifact@v3
        with:
          path: ./publish/wwwroot

  deploy:
    needs: build
    permissions:
      pages: write
      id-token: write
    environment:
      name: github-pages
    runs-on: ubuntu-latest
    steps:
      - uses: actions/deploy-pages@v4
```

---

## Migration Strategy for Existing Pages

Existing hard-coded pages (`Aenar.razor`, `V1Page.razor`, etc.) can be kept as-is while the
data layer is built. The migration can be done article-by-article:

1. Extract the hard-coded content from `Aenar.razor` into `aenar.json`.
2. Verify the dynamic page renders the same output.
3. Delete the old static `.razor` page.
4. Repeat for each article.

This allows incremental migration with no downtime.

---

## Open Questions

- **Markdown vs JSON**: Body text currently contains inline components (e.g. `<IPAOption>`).
  These cannot be expressed in plain Markdown. Options:
  - Use JSON with a structured section model (see Phase 2 above).
  - Introduce a lightweight MDX-style syntax and a custom renderer.
  - Keep body text as HTML strings rendered via `MarkupString`.

- **Interactive islands**: Some pages may need client-side interactivity in future (e.g. search,
  language switcher). With .NET 8 Blazor, individual components can opt in to
  `InteractiveWebAssembly` rendering while the rest of the page stays static.

- **Article discovery**: The dynamic page needs a way to enumerate all article IDs to
  pre-render them at publish time. `JsonArticleService` can expose a `ListAsync()` method
  that reads all JSON files in the content directory.

---

## Summary

| Phase | Description | Effort |
|---|---|---|
| 1 | Upgrade to .NET 8 | Low |
| 2 | Content data layer + JSON files | Medium |
| 3 | Dynamic `ArticlePage.razor` | Medium |
| 4 | Static publish + GitHub Actions deploy | Low |
