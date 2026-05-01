namespace Athena.Content.Models;

public record Article(
    string Id,
    string Title,
    IReadOnlyList<LanguageAlternative> LanguageNames,
    IReadOnlyList<ArticleSection> Sections,
    Infobox? Infobox,
    IReadOnlyList<RelatedLink> RelatedArticles);
