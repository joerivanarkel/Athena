using System.Net.Http.Json;
using System.Text.Json;
using Athena.Content.Models;

namespace Athena.Content.Services;

public class JsonArticleService : IArticleService
{
    private readonly HttpClient _http;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public JsonArticleService(HttpClient http)
    {
        _http = http;
    }

    public async Task<Article?> GetAsync(string id)
    {
        try
        {
            return await _http.GetFromJsonAsync<Article>(
                $"content/{id.ToLowerInvariant()}.json",
                JsonOptions);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}
