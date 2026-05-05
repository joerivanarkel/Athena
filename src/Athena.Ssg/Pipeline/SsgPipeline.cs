using Athena.Ssg.Pages;
using Athena.Ssg.Rendering;
using Athena.Ssg.Templates;

namespace Athena.Ssg.Pipeline;

internal sealed class SsgPipeline
{
    private readonly ContentService _content;
    private readonly string _outputDir;
    private readonly string _baseHref;

    public SsgPipeline(ContentService content, string outputDir, string baseHref)
    {
        _content = content;
        _outputDir = outputDir;
        _baseHref = baseHref;
    }

    public async Task RunAsync(string assetsSourceDir)
    {
        Directory.CreateDirectory(_outputDir);

        await using var renderer = new ComponentRenderer(_baseHref);

        // Render each article
        foreach (var document in _content.All)
        {
            Console.WriteLine($"  Rendering: {document.Meta.Slug}");

            var html = await renderer.RenderAsync<AthenaArticlePage>(
                new Dictionary<string, object?> { ["Document"] = document });

            var page = HtmlShell.Wrap(document.Meta.Title, html, _baseHref);
            var dir = Path.Combine(_outputDir, document.Meta.Slug);
            Directory.CreateDirectory(dir);
            await File.WriteAllTextAsync(Path.Combine(dir, "index.html"), page);
        }

        // Render index
        Console.WriteLine("  Rendering: index");
        var indexHtml = await renderer.RenderAsync<AthenaIndexPage>(
            new Dictionary<string, object?> { ["Articles"] = _content.All });
        var indexPage = HtmlShell.WrapIndex(indexHtml, _baseHref);
        await File.WriteAllTextAsync(Path.Combine(_outputDir, "index.html"), indexPage);

        // Copy static assets
        CopyAssets(assetsSourceDir, _outputDir);

        Console.WriteLine($"  Done. Output: {_outputDir}");
    }

    private static void CopyAssets(string sourceDir, string destDir)
    {
        if (!Directory.Exists(sourceDir))
        {
            Console.WriteLine($"  Warning: assets directory not found: {sourceDir}");
            return;
        }

        foreach (var file in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
        {
            var relative = Path.GetRelativePath(sourceDir, file);
            var dest = Path.Combine(destDir, relative);
            Directory.CreateDirectory(Path.GetDirectoryName(dest)!);
            File.Copy(file, dest, overwrite: true);
        }
    }
}
