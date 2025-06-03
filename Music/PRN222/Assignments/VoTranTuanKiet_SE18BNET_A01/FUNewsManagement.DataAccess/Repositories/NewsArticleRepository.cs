using Microsoft.EntityFrameworkCore;
using FUNewsManagement.Models;

namespace FUNewsManagement.DataAccess.Repositories
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        private readonly FUNewsManagementDbContext _context;

        public NewsArticleRepository(FUNewsManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NewsArticle>> GetAllAsync()
        {
            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetActiveAsync()
        {
            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .Where(n => n.NewsStatus == true)
                .ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetByCreatedByAsync(short createdById)
        {
            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .Where(n => n.CreatedById == createdById)
                .ToListAsync();
        }

        public async Task<NewsArticle?> GetByIdAsync(string id)
        {
            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .FirstOrDefaultAsync(n => n.NewsArticleId == id);
        }

        public async Task<NewsArticle> AddAsync(NewsArticle newsArticle)
        {
            _context.NewsArticles.Add(newsArticle);
            await _context.SaveChangesAsync();
            return newsArticle;
        }

        public async Task<NewsArticle> UpdateAsync(NewsArticle newsArticle)
        {
            newsArticle.ModifiedDate = DateTime.Now;
            _context.NewsArticles.Update(newsArticle);
            await _context.SaveChangesAsync();
            return newsArticle;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var newsArticle = await _context.NewsArticles.FindAsync(id);
            if (newsArticle == null) return false;

            _context.NewsArticles.Remove(newsArticle);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _context.NewsArticles.AnyAsync(n => n.NewsArticleId == id);
        }

        public async Task<IEnumerable<NewsArticle>> SearchAsync(string searchTerm)
        {
            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .Where(n => n.NewsTitle.Contains(searchTerm) || 
                           (n.NewsContent != null && n.NewsContent.Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetByCategoryAsync(short categoryId)
        {
            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .Where(n => n.CategoryId == categoryId)
                .ToListAsync();
        }
    }
}
