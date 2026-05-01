namespace Athena.Content.Models;

public record Infobox(
    string Title,
    IReadOnlyList<LanguageAlternative> LanguageNames,
    IReadOnlyList<InfoboxDetail> Details);
