using Athena.Content.Models;

namespace Athena.Content.Services;

public interface IArticleService
{
    Task<Article?> GetAsync(string id);
}
