using Microsoft.EntityFrameworkCore;
using FUNewsManagement.Models;

namespace FUNewsManagement.DataAccess.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly FUNewsManagementDbContext _context;

        public TagRepository(FUNewsManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tags
                .Include(t => t.NewsArticle)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tag>> GetByNewsArticleIdAsync(string newsArticleId)
        {
            return await _context.Tags
                .Where(t => t.NewsArticleId == newsArticleId)
                .ToListAsync();
        }

        public async Task<Tag?> GetByIdAsync(int id)
        {
            return await _context.Tags
                .Include(t => t.NewsArticle)
                .FirstOrDefaultAsync(t => t.TagId == id);
        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag> UpdateAsync(Tag tag)
        {
            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return false;

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteByNewsArticleIdAsync(string newsArticleId)
        {
            var tags = await _context.Tags
                .Where(t => t.NewsArticleId == newsArticleId)
                .ToListAsync();

            if (tags.Any())
            {
                _context.Tags.RemoveRange(tags);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Tags.AnyAsync(t => t.TagId == id);
        }

        public async Task<IEnumerable<Tag>> SearchAsync(string searchTerm)
        {
            return await _context.Tags
                .Include(t => t.NewsArticle)
                .Where(t => t.TagName.Contains(searchTerm) || 
                           (t.Note != null && t.Note.Contains(searchTerm)))
                .ToListAsync();
        }
    }
}
