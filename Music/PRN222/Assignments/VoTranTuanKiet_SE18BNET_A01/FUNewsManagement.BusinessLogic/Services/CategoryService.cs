using FUNewsManagement.DataAccess.Repositories;
using FUNewsManagement.Models;

namespace FUNewsManagement.BusinessLogic.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _categoryRepository.GetActiveAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(short id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            if (!await ValidateCategoryDataAsync(category))
            {
                throw new ArgumentException("Invalid category data");
            }

            return await _categoryRepository.AddAsync(category);
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            if (!await ValidateCategoryDataAsync(category))
            {
                throw new ArgumentException("Invalid category data");
            }

            return await _categoryRepository.UpdateAsync(category);
        }

        public async Task<bool> DeleteCategoryAsync(short id)
        {
            if (!await CanDeleteCategoryAsync(id))
            {
                throw new InvalidOperationException("Cannot delete category that has associated news articles");
            }

            return await _categoryRepository.DeleteAsync(id);
        }

        public async Task<bool> CategoryExistsAsync(short id)
        {
            return await _categoryRepository.ExistsAsync(id);
        }

        public async Task<bool> CanDeleteCategoryAsync(short id)
        {
            return !await _categoryRepository.HasNewsArticlesAsync(id);
        }

        public async Task<IEnumerable<Category>> SearchCategoriesAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllCategoriesAsync();
            }

            return await _categoryRepository.SearchAsync(searchTerm);
        }

        public async Task<bool> ValidateCategoryDataAsync(Category category)
        {
            if (category == null) return false;
            if (string.IsNullOrWhiteSpace(category.CategoryName)) return false;
            if (category.CategoryName.Length > 100) return false;
            if (category.CategoryDesciption != null && category.CategoryDesciption.Length > 250) return false;

            return true;
        }
    }
}
