using Athena.Ssg.Pipeline;

// ── Parse arguments ────────────────────────────────────────────────────────
var contentDir = Arg("--content-dir", args) ?? Path.Combine(AppContext.BaseDirectory, "Content");
var outputDir  = Arg("--output-dir",  args) ?? Path.Combine(AppContext.BaseDirectory, "output");
var baseHref   = Arg("--base-href",   args) ?? "/";

// The assets directory is the wwwroot of Athena.Presentation.Components,
// which is embedded into the output as _content/ during publish.
// At dev time, pass its path explicitly so CSS is available in local-output.
var assetsDir  = Arg("--assets-dir",  args) ?? string.Empty;

Console.WriteLine($"Athena SSG");
Console.WriteLine($"  content : {contentDir}");
Console.WriteLine($"  output  : {outputDir}");
Console.WriteLine($"  baseHref: {baseHref}");

// ── Load content ────────────────────────────────────────────────────────────
var content = new ContentService(contentDir);
Console.WriteLine($"  loaded  : {content.All.Count} article(s)");

// ── Run pipeline ───────────────────────────────────────────────────────────
var pipeline = new SsgPipeline(content, outputDir, baseHref);
await pipeline.RunAsync(assetsDir);

// ── Helpers ────────────────────────────────────────────────────────────────
static string? Arg(string name, string[] args)
{
    var idx = Array.IndexOf(args, name);
    return idx >= 0 && idx + 1 < args.Length ? args[idx + 1] : null;
}
