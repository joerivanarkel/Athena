namespace Athena.Ssg.Templates;

internal static class HtmlShell
{
    public static string Wrap(string title, string body, string baseHref) => $"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="utf-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            <title>{System.Net.WebUtility.HtmlEncode(title)} — Athena</title>
            <base href="{baseHref}" />
            <link href="css/bootstrap/bootstrap.min.css" rel="stylesheet" />
            <link href="css/app.css" rel="stylesheet" />
            <link href="css/Athena.Presentation.Components.css" rel="stylesheet" />
        </head>
        <body class="bg-gray-50">
            <div class="max-w-7xl mx-auto px-4 py-6">
                {body}
            </div>
        </body>
        </html>
        """;

    public static string WrapIndex(string body, string baseHref) => $"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="utf-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            <title>Athena</title>
            <base href="{baseHref}" />
            <link href="css/bootstrap/bootstrap.min.css" rel="stylesheet" />
            <link href="css/app.css" rel="stylesheet" />
            <link href="css/Athena.Presentation.Components.css" rel="stylesheet" />
        </head>
        <body class="bg-gray-50">
            <div class="max-w-7xl mx-auto px-4 py-6">
                {body}
            </div>
        </body>
        </html>
        """;
}
