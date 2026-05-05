using Microsoft.AspNetCore.Components;

namespace Athena.Ssg.Rendering;

/// <summary>
/// Stub NavigationManager for static rendering. HeaderLink uses it to strip
/// the current path before appending an anchor, so it only needs a stable base URI.
/// </summary>
internal sealed class SsgNavigationManager : NavigationManager
{
    public SsgNavigationManager(string baseHref)
    {
        // NavigationManager requires an absolute URI. Convert a path like "/Athena/" to
        // "http://localhost/Athena/" so the stub initializes without throwing UriFormatException.
        var absolute = baseHref.StartsWith("http", StringComparison.OrdinalIgnoreCase)
            ? baseHref
            : "http://localhost" + (baseHref.StartsWith('/') ? baseHref : "/" + baseHref);
        var uri = absolute.TrimEnd('/') + "/";
        Initialize(uri, uri);
    }

    protected override void NavigateToCore(string uri, NavigationOptions options) { }
}
