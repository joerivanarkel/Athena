namespace Athena.Content.Models;

public record AthenaMeta(
    string Title,
    string Slug,
    string Category,
    IReadOnlyList<string> Tags,
    AthenaInfobox? Infobox
);
