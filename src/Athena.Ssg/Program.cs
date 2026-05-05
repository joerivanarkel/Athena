using Athena.Ssg.Pipeline;

var sw = System.Diagnostics.Stopwatch.StartNew();

// ── Resolve paths ──────────────────────────────────────────────────────────
// `dotnet run --project` sets CWD to the project dir, not the repo root.
// ResolveFromRepoRoot walks up from CWD to find the repo root (the directory
// that contains Athena.sln), then resolves relative paths from there.
var repoRoot = FindRepoRoot(Directory.GetCurrentDirectory());

var contentDir = Resolve(Arg("--content-dir", args), repoRoot)
    ?? Path.Combine(AppContext.BaseDirectory, "Content");
var outputDir  = Resolve(Arg("--output-dir",  args), repoRoot)
    ?? Path.Combine(AppContext.BaseDirectory, "output");
var baseHref   = Arg("--base-href",   args) ?? "/";
var assetsDir  = Resolve(Arg("--assets-dir", args), repoRoot) ?? string.Empty;

Console.WriteLine("Athena SSG");
Console.WriteLine($"  repo root : {repoRoot ?? "(not found, using CWD)"}");
Console.WriteLine($"  content   : {contentDir}");
Console.WriteLine($"  output    : {outputDir}");
Console.WriteLine($"  base href : {baseHref}");

// ── Load content ────────────────────────────────────────────────────────────
var content = new ContentService(contentDir);
Console.WriteLine($"  articles  : {content.All.Count}");

// ── Run pipeline ───────────────────────────────────────────────────────────
var pipeline = new SsgPipeline(content, outputDir, baseHref);
await pipeline.RunAsync(assetsDir);

Console.WriteLine($"  elapsed   : {sw.ElapsedMilliseconds} ms");

// ── Helpers ────────────────────────────────────────────────────────────────
static string? Arg(string name, string[] args)
{
    var idx = Array.IndexOf(args, name);
    return idx >= 0 && idx + 1 < args.Length ? args[idx + 1] : null;
}

// Walk up from startDir to find the directory containing Athena.sln.
static string? FindRepoRoot(string startDir)
{
    var dir = startDir;
    while (dir is not null)
    {
        if (File.Exists(Path.Combine(dir, "Athena.sln")))
            return dir;
        dir = Path.GetDirectoryName(dir);
    }
    return null;
}

// Resolve a path: absolute paths are used as-is; relative paths are resolved
// against the repo root (if found) then against the CWD as fallback.
static string? Resolve(string? path, string? repoRoot)
{
    if (path is null) return null;
    if (Path.IsPathRooted(path)) return path;
    var bases = new[] { repoRoot, Directory.GetCurrentDirectory() }.Where(b => b is not null);
    return bases.Select(b => Path.GetFullPath(path, b!)).FirstOrDefault();
}
