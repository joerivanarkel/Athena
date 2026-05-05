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
        Console.WriteLine($"  rendering : {_content.All.Count} article(s) + index → {_outputDir}");

        await using var renderer = new ComponentRenderer(_baseHref);
        var sw = System.Diagnostics.Stopwatch.StartNew();

        foreach (var document in _content.All)
        {
            sw.Restart();
            var html = await renderer.RenderAsync<AthenaArticlePage>(
                new Dictionary<string, object?> { ["Document"] = document });
            var page = HtmlShell.Wrap(document.Meta.Title, html, _baseHref);
            var dir = Path.Combine(_outputDir, document.Meta.Slug);
            Directory.CreateDirectory(dir);
            await File.WriteAllTextAsync(Path.Combine(dir, "index.html"), page);
            Console.WriteLine($"    [ok] /{document.Meta.Slug}/ ({sw.ElapsedMilliseconds} ms, {page.Length:N0} bytes)");
        }

        sw.Restart();
        var indexHtml = await renderer.RenderAsync<AthenaIndexPage>(
            new Dictionary<string, object?> { ["Articles"] = _content.All });
        var indexPage = HtmlShell.WrapIndex(indexHtml, _baseHref);
        await File.WriteAllTextAsync(Path.Combine(_outputDir, "index.html"), indexPage);
        Console.WriteLine($"    [ok] / ({sw.ElapsedMilliseconds} ms, {indexPage.Length:N0} bytes)");

        CopyAssets(assetsSourceDir, _outputDir);
    }

    private static void CopyAssets(string sourceDir, string destDir)
    {
        if (string.IsNullOrEmpty(sourceDir) || !Directory.Exists(sourceDir))
        {
            if (!string.IsNullOrEmpty(sourceDir))
                Console.WriteLine($"  assets    : not found at {sourceDir} (skipping)");
            return;
        }

        var files = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            var relative = Path.GetRelativePath(sourceDir, file);
            var dest = Path.Combine(destDir, relative);
            Directory.CreateDirectory(Path.GetDirectoryName(dest)!);
            File.Copy(file, dest, overwrite: true);
        }
        Console.WriteLine($"  assets    : copied {files.Length} file(s) from {sourceDir}");
    }
}
