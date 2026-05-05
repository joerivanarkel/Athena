namespace Athena.Content.Models;

public record AthenaDocument(
    AthenaMeta Meta,
    IReadOnlyList<AthenaSection> Sections,
    IReadOnlyList<AthenaFootnote> Footnotes
);
