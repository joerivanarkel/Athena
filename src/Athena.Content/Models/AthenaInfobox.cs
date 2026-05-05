namespace Athena.Content.Models;

public record AthenaInfobox(
    string? Title,
    IReadOnlyList<AthenaLanguage> Languages,
    AthenaImage? Image,
    IReadOnlyList<AthenaDetail> Details
);

public record AthenaLanguage(string Name, string Language);

public record AthenaImage(string Src, string Alt);

public record AthenaDetail(string Item, string Value);
