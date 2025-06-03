using Microsoft.EntityFrameworkCore;
using FUNewsManagement.Models;

namespace FUNewsManagement.DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FUNewsManagementDbContext _context;

        public CategoryRepository(FUNewsManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetActiveAsync()
        {
            return await _context.Categories
                .Where(c => c.IsActive == true)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(short id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(short id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(short id)
        {
            return await _context.Categories.AnyAsync(c => c.CategoryId == id);
        }

        public async Task<bool> HasNewsArticlesAsync(short id)
        {
            return await _context.NewsArticles.AnyAsync(n => n.CategoryId == id);
        }

        public async Task<IEnumerable<Category>> SearchAsync(string searchTerm)
        {
            return await _context.Categories
                .Where(c => c.CategoryName.Contains(searchTerm) || 
                           (c.CategoryDesciption != null && c.CategoryDesciption.Contains(searchTerm)))
                .ToListAsync();
        }
    }
}
