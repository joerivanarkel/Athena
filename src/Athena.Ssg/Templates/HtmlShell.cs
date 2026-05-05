namespace Athena.Ssg.Templates;

internal static class HtmlShell
{
    public static string Wrap(string title, string body, string baseHref) => $"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="utf-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            <title>{System.Net.WebUtility.HtmlEncode(title)} — Codex Mundi</title>
            <base href="{baseHref}" />
            <link rel="preconnect" href="https://fonts.googleapis.com" />
            <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="anonymous" />
            <link href="https://fonts.googleapis.com/css2?family=EB+Garamond:ital,wght@0,400;0,500;0,600;0,700;1,400;1,500&family=Inter:wght@400;500;600&family=JetBrains+Mono:wght@400;500&display=swap" rel="stylesheet" />
            <link href="css/codex.css" rel="stylesheet" />
        </head>
        <body>
            {body}
        </body>
        </html>
        """;

    public static string WrapIndex(string body, string baseHref) => $"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="utf-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            <title>Codex Mundi</title>
            <base href="{baseHref}" />
            <link rel="preconnect" href="https://fonts.googleapis.com" />
            <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="anonymous" />
            <link href="https://fonts.googleapis.com/css2?family=EB+Garamond:ital,wght@0,400;0,500;0,600;0,700;1,400;1,500&family=Inter:wght@400;500;600&family=JetBrains+Mono:wght@400;500&display=swap" rel="stylesheet" />
            <link href="css/codex.css" rel="stylesheet" />
        </head>
        <body>
            {body}
        </body>
        </html>
        """;
}
