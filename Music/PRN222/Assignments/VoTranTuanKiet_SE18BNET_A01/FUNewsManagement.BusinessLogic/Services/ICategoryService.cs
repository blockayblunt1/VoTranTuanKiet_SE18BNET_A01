using FUNewsManagement.Models;

namespace FUNewsManagement.BusinessLogic.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(short id);
        Task<Category> CreateCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(short id);
        Task<bool> CategoryExistsAsync(short id);
        Task<bool> CanDeleteCategoryAsync(short id);
        Task<IEnumerable<Category>> SearchCategoriesAsync(string searchTerm);
        Task<bool> ValidateCategoryDataAsync(Category category);
    }
}
