# Copilot Instructions

## Decision Tracking

When a non-trivial architectural or naming decision is made, document it in
`docs/decisions/` as `YYYY-MM-DD-short-title.md` with three sections:

- **Context** — what situation or constraint prompted the decision
- **Decision** — what was decided and why
- **Consequences** — trade-offs or follow-on effects

## Documentation First

Before adding a new component, add its entry to the relevant file in `docs/components/`.
After completing a component, verify the docs match the final parameter list.

## Docs Folder Awareness

The `docs/` folder is the source of truth for this project. Always analyze it when asked
about component usage, the `.athena` format, or project structure.

- `docs/components/` — per-group component parameter reference
- `docs/format/` — `.athena` content format specification
- `docs/decisions/` — architecture decision records

## Namespace Convention

All projects in this solution use the `Athena.` prefix:

- Component library: `Athena.Presentation.Components.Wrapper`
- Content models: `Athena.Content.Models`
- Content parser: `Athena.Content.Parsing`
- SSG: `Athena.Ssg`

## Content Format

The `.athena` format is a custom Athena-native format. Refer to `docs/format/athena-format.md`
for the full specification before generating or modifying `.athena` content files.

## Technology Stack

- .NET 9, Blazor (WebAssembly for dev, Static SSR for production)
- TailwindCSS v3 + SCSS for styling
- YamlDotNet for frontmatter parsing
- Markdig for CommonMark rendering
