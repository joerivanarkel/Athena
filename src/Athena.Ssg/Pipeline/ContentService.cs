using Athena.Content.Models;
using Athena.Content.Parsing;

namespace Athena.Ssg.Pipeline;

internal sealed class ContentService
{
    private readonly IReadOnlyList<AthenaDocument> _documents;

    public ContentService(string contentDirectory)
    {
        if (!Directory.Exists(contentDirectory))
            throw new DirectoryNotFoundException(
                $"Content directory not found: {contentDirectory}\n" +
                $"  CWD: {Directory.GetCurrentDirectory()}\n" +
                $"  Hint: pass an absolute path or run from the repo root.");

        var files = Directory.GetFiles(contentDirectory, "*.athena", SearchOption.AllDirectories);
        Console.WriteLine($"  parsing   : {files.Length} file(s) from {contentDirectory}");

        var documents = new List<AthenaDocument>();
        foreach (var file in files)
        {
            try
            {
                var doc = AthenaParser.Parse(file);
                Console.WriteLine($"    [ok] {doc.Meta.Slug} — \"{doc.Meta.Title}\" ({doc.Sections.Count} sections, {doc.Footnotes.Count} footnotes)");
                documents.Add(doc);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"    [FAIL] {Path.GetFileName(file)}: {ex.Message}");
            }
        }

        _documents = documents;
    }

    public IReadOnlyList<AthenaDocument> All => _documents;

    public AthenaDocument? GetBySlug(string slug) =>
        _documents.FirstOrDefault(d => d.Meta.Slug == slug);
}
