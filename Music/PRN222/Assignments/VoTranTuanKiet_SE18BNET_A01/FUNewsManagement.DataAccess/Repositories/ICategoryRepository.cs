using FUNewsManagement.Models;

namespace FUNewsManagement.DataAccess.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<IEnumerable<Category>> GetActiveAsync();
        Task<Category?> GetByIdAsync(short id);
        Task<Category> AddAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task<bool> DeleteAsync(short id);
        Task<bool> ExistsAsync(short id);
        Task<bool> HasNewsArticlesAsync(short id);
        Task<IEnumerable<Category>> SearchAsync(string searchTerm);
    }
}
