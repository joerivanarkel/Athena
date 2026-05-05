using System.Text;
using System.Text.RegularExpressions;
using Athena.Content.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Athena.Content.Parsing;

public static class AthenaParser
{
    private static readonly IDeserializer YamlDeserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();

    private static readonly Regex FootnoteDefPattern =
        new(@"^\[\^(?<key>[^\]]+)\]:\s*(?<text>.+)$", RegexOptions.Multiline);

    private static readonly Regex ObsidianCommentPattern =
        new(@"%%[^%]*%%", RegexOptions.Singleline);

    // Strips Obsidian ![[embed]] syntax (images, transclusions) not supported by the SSG.
    private static readonly Regex ObsidianEmbedPattern =
        new(@"!\[\[[^\]]*\]\]", RegexOptions.None);

    private static readonly Regex HeadingPattern =
        new(@"^(?<hashes>#{1,3})\s+(?<title>.+)$", RegexOptions.Multiline);

    public static AthenaDocument Parse(string filePath)
    {
        var raw = File.ReadAllText(filePath, Encoding.UTF8);
        return ParseContent(raw);
    }

    public static AthenaDocument ParseContent(string content)
    {
        var (frontmatter, body) = SplitFrontmatter(content);
        var meta = ParseFrontmatter(frontmatter);

        body = ObsidianCommentPattern.Replace(body, string.Empty);
        body = ObsidianEmbedPattern.Replace(body, string.Empty);

        var footnotes = ExtractFootnotes(ref body);
        var sections = BuildSectionTree(body);

        return new AthenaDocument(meta, sections, footnotes);
    }

    private static (string Frontmatter, string Body) SplitFrontmatter(string content)
    {
        var trimmed = content.TrimStart();
        if (!trimmed.StartsWith("---"))
            return (string.Empty, content);

        var first = trimmed.IndexOf('\n') + 1;
        var second = trimmed.IndexOf("\n---", first);
        if (second < 0)
            return (string.Empty, content);

        var frontmatter = trimmed[first..second].Trim();
        var body = trimmed[(second + 4)..].TrimStart('\r', '\n');
        return (frontmatter, body);
    }

    private static AthenaMeta ParseFrontmatter(string yaml)
    {
        if (string.IsNullOrWhiteSpace(yaml))
            return new AthenaMeta("Untitled", "untitled", string.Empty, [], null);

        var raw = YamlDeserializer.Deserialize<RawFrontmatter>(yaml);

        var infobox = raw.Infobox is null ? null : new AthenaInfobox(
            raw.Infobox.Title,
            raw.Infobox.Languages?.Select(l => new AthenaLanguage(l.Name ?? "", l.Language ?? "")).ToList() ?? [],
            raw.Infobox.Image is null ? null : new AthenaImage(raw.Infobox.Image.Src ?? "", raw.Infobox.Image.Alt ?? ""),
            raw.Infobox.Details?.Select(d => new AthenaDetail(d.Item ?? "", d.Value ?? "")).ToList() ?? []
        );

        return new AthenaMeta(
            Title: raw.Title ?? "Untitled",
            Slug: raw.Slug ?? Slugify(raw.Title ?? "untitled"),
            Category: raw.Category ?? string.Empty,
            Tags: raw.Tags ?? [],
            Infobox: infobox
        );
    }

    private static IReadOnlyList<AthenaFootnote> ExtractFootnotes(ref string body)
    {
        var footnotes = new List<AthenaFootnote>();
        body = FootnoteDefPattern.Replace(body, m =>
        {
            footnotes.Add(new AthenaFootnote(m.Groups["key"].Value, m.Groups["text"].Value.Trim()));
            return string.Empty;
        });
        body = body.TrimEnd();
        return footnotes;
    }

    private static IReadOnlyList<AthenaSection> BuildSectionTree(string body)
    {
        // Split body into segments: intro block (before first heading) + headed sections
        var matches = HeadingPattern.Matches(body);

        if (matches.Count == 0)
            return [new AthenaSection("Introduction", 0, body.Trim(), [])];

        var flat = new List<(int Level, string Name, string MarkdownBody)>();

        // Content before first heading is the intro (level 0, no title)
        var intro = body[..matches[0].Index].Trim();
        if (!string.IsNullOrWhiteSpace(intro))
            flat.Add((0, string.Empty, intro));

        for (var i = 0; i < matches.Count; i++)
        {
            var m = matches[i];
            var level = m.Groups["hashes"].Length;
            var title = m.Groups["title"].Value.Trim();
            var start = m.Index + m.Length;
            var end = i + 1 < matches.Count ? matches[i + 1].Index : body.Length;
            var sectionBody = body[start..end].Trim();
            flat.Add((level, title, sectionBody));
        }

        return BuildTree(flat, 0, flat.Count);
    }

    private static IReadOnlyList<AthenaSection> BuildTree(
        List<(int Level, string Name, string MarkdownBody)> flat, int start, int end)
    {
        var result = new List<AthenaSection>();
        var i = start;

        while (i < end)
        {
            var (level, name, mdBody) = flat[i];

            // Level-0 intro block is always a standalone leaf — never claims children.
            // For headed sections, gather everything at a strictly deeper level.
            var childEnd = i + 1;
            if (level > 0)
            {
                while (childEnd < end && flat[childEnd].Level > level)
                    childEnd++;
            }

            var children = (level == 0 || childEnd == i + 1)
                ? (IReadOnlyList<AthenaSection>)[]
                : BuildTree(flat, i + 1, childEnd);

            result.Add(new AthenaSection(name, level, mdBody, children));
            i = childEnd;
        }

        return result;
    }

    private static string Slugify(string title) =>
        Regex.Replace(title.ToLowerInvariant().Trim(), @"[^a-z0-9]+", "-").Trim('-');

    // ── YAML deserialization targets ────────────────────────────────────────

    private sealed class RawFrontmatter
    {
        public string? Title { get; set; }
        public string? Slug { get; set; }
        public string? Category { get; set; }
        public List<string>? Tags { get; set; }
        public RawInfobox? Infobox { get; set; }
    }

    private sealed class RawInfobox
    {
        public string? Title { get; set; }
        public List<RawLanguage>? Languages { get; set; }
        public RawImage? Image { get; set; }
        public List<RawDetail>? Details { get; set; }
    }

    private sealed class RawLanguage
    {
        public string? Name { get; set; }
        public string? Language { get; set; }
    }

    private sealed class RawImage
    {
        public string? Src { get; set; }
        public string? Alt { get; set; }
    }

    private sealed class RawDetail
    {
        public string? Item { get; set; }
        public string? Value { get; set; }
    }
}
