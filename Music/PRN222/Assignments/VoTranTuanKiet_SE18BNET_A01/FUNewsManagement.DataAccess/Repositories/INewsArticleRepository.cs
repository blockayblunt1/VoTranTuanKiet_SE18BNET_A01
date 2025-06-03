using FUNewsManagement.Models;

namespace FUNewsManagement.DataAccess.Repositories
{
    public interface INewsArticleRepository
    {
        Task<IEnumerable<NewsArticle>> GetAllAsync();
        Task<IEnumerable<NewsArticle>> GetActiveAsync();
        Task<IEnumerable<NewsArticle>> GetByCreatedByAsync(short createdById);
        Task<NewsArticle?> GetByIdAsync(string id);
        Task<NewsArticle> AddAsync(NewsArticle newsArticle);
        Task<NewsArticle> UpdateAsync(NewsArticle newsArticle);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<IEnumerable<NewsArticle>> SearchAsync(string searchTerm);
        Task<IEnumerable<NewsArticle>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<NewsArticle>> GetByCategoryAsync(short categoryId);
    }
}
