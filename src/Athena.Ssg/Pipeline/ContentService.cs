using Athena.Content.Models;
using Athena.Content.Parsing;

namespace Athena.Ssg.Pipeline;

internal sealed class ContentService
{
    private readonly IReadOnlyList<AthenaDocument> _documents;

    public ContentService(string contentDirectory)
    {
        var files = Directory.GetFiles(contentDirectory, "*.athena", SearchOption.AllDirectories);
        _documents = files.Select(AthenaParser.Parse).ToList();
    }

    public IReadOnlyList<AthenaDocument> All => _documents;

    public AthenaDocument? GetBySlug(string slug) =>
        _documents.FirstOrDefault(d => d.Meta.Slug == slug);
}
