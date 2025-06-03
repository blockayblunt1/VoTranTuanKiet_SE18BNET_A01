using FUNewsManagement.Models;

namespace FUNewsManagement.BusinessLogic.Services
{
    public interface INewsArticleService
    {
        Task<IEnumerable<NewsArticle>> GetAllNewsArticlesAsync();
        Task<IEnumerable<NewsArticle>> GetActiveNewsArticlesAsync();
        Task<IEnumerable<NewsArticle>> GetNewsArticlesByCreatedByAsync(short createdById);
        Task<NewsArticle?> GetNewsArticleByIdAsync(string id);
        Task<NewsArticle> CreateNewsArticleAsync(NewsArticle newsArticle, List<string> tagNames);
        Task<NewsArticle> UpdateNewsArticleAsync(NewsArticle newsArticle, List<string> tagNames);
        Task<bool> DeleteNewsArticleAsync(string id);
        Task<bool> NewsArticleExistsAsync(string id);
        Task<IEnumerable<NewsArticle>> SearchNewsArticlesAsync(string searchTerm);
        Task<IEnumerable<NewsArticle>> GetNewsArticlesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<NewsArticle>> GetNewsArticlesByCategoryAsync(short categoryId);
        Task<bool> ValidateNewsArticleDataAsync(NewsArticle newsArticle);
        Task<string> GenerateNewsArticleIdAsync();
    }
}
