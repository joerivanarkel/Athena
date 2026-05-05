namespace Athena.Content.Models;

/// <summary>
/// A footnote definition from the document body. Used for both IPA transcriptions and references.
/// Key matches the [^key] inline reference; Text is the footnote content.
/// </summary>
public record AthenaFootnote(string Key, string Text);
