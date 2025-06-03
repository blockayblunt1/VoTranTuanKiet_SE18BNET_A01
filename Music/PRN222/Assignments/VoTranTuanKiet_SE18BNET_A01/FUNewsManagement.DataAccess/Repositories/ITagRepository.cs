using FUNewsManagement.Models;

namespace FUNewsManagement.DataAccess.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<IEnumerable<Tag>> GetByNewsArticleIdAsync(string newsArticleId);
        Task<Tag?> GetByIdAsync(int id);
        Task<Tag> AddAsync(Tag tag);
        Task<Tag> UpdateAsync(Tag tag);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByNewsArticleIdAsync(string newsArticleId);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Tag>> SearchAsync(string searchTerm);
    }
}
