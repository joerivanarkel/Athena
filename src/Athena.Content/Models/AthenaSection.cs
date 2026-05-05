namespace Athena.Content.Models;

public record AthenaSection(
    string Name,
    int Level,
    string MarkdownBody,
    IReadOnlyList<AthenaSection> Children
);
