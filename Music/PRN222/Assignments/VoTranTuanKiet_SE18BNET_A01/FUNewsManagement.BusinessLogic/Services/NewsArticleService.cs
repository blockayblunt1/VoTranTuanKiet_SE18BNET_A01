using FUNewsManagement.DataAccess.Repositories;
using FUNewsManagement.Models;

namespace FUNewsManagement.BusinessLogic.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICategoryRepository _categoryRepository;

        public NewsArticleService(
            INewsArticleRepository newsArticleRepository,
            ITagRepository tagRepository,
            ICategoryRepository categoryRepository)
        {
            _newsArticleRepository = newsArticleRepository;
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<NewsArticle>> GetAllNewsArticlesAsync()
        {
            return await _newsArticleRepository.GetAllAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetActiveNewsArticlesAsync()
        {
            return await _newsArticleRepository.GetActiveAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsArticlesByCreatedByAsync(short createdById)
        {
            return await _newsArticleRepository.GetByCreatedByAsync(createdById);
        }

        public async Task<NewsArticle?> GetNewsArticleByIdAsync(string id)
        {
            return await _newsArticleRepository.GetByIdAsync(id);
        }

        public async Task<NewsArticle> CreateNewsArticleAsync(NewsArticle newsArticle, List<string> tagNames)
        {
            if (!await ValidateNewsArticleDataAsync(newsArticle))
            {
                throw new ArgumentException("Invalid news article data");
            }

            if (string.IsNullOrWhiteSpace(newsArticle.NewsArticleId))
            {
                newsArticle.NewsArticleId = await GenerateNewsArticleIdAsync();
            }

            if (await _newsArticleRepository.ExistsAsync(newsArticle.NewsArticleId))
            {
                throw new ArgumentException("News article ID already exists");
            }

            newsArticle.CreatedDate = DateTime.Now;
            var createdArticle = await _newsArticleRepository.AddAsync(newsArticle);

            // Add tags
            if (tagNames != null && tagNames.Any())
            {
                foreach (var tagName in tagNames.Where(t => !string.IsNullOrWhiteSpace(t)))
                {
                    var tag = new Tag
                    {
                        TagName = tagName.Trim(),
                        NewsArticleId = createdArticle.NewsArticleId
                    };
                    await _tagRepository.AddAsync(tag);
                }
            }

            return await _newsArticleRepository.GetByIdAsync(createdArticle.NewsArticleId) ?? createdArticle;
        }

        public async Task<NewsArticle> UpdateNewsArticleAsync(NewsArticle newsArticle, List<string> tagNames)
        {
            if (!await ValidateNewsArticleDataAsync(newsArticle))
            {
                throw new ArgumentException("Invalid news article data");
            }

            // Delete existing tags
            await _tagRepository.DeleteByNewsArticleIdAsync(newsArticle.NewsArticleId);

            // Add new tags
            if (tagNames != null && tagNames.Any())
            {
                foreach (var tagName in tagNames.Where(t => !string.IsNullOrWhiteSpace(t)))
                {
                    var tag = new Tag
                    {
                        TagName = tagName.Trim(),
                        NewsArticleId = newsArticle.NewsArticleId
                    };
                    await _tagRepository.AddAsync(tag);
                }
            }

            return await _newsArticleRepository.UpdateAsync(newsArticle);
        }

        public async Task<bool> DeleteNewsArticleAsync(string id)
        {
            return await _newsArticleRepository.DeleteAsync(id);
        }

        public async Task<bool> NewsArticleExistsAsync(string id)
        {
            return await _newsArticleRepository.ExistsAsync(id);
        }

        public async Task<IEnumerable<NewsArticle>> SearchNewsArticlesAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllNewsArticlesAsync();
            }

            return await _newsArticleRepository.SearchAsync(searchTerm);
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsArticlesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _newsArticleRepository.GetByDateRangeAsync(startDate, endDate);
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsArticlesByCategoryAsync(short categoryId)
        {
            return await _newsArticleRepository.GetByCategoryAsync(categoryId);
        }

        public async Task<bool> ValidateNewsArticleDataAsync(NewsArticle newsArticle)
        {
            if (newsArticle == null) return false;
            if (string.IsNullOrWhiteSpace(newsArticle.NewsTitle)) return false;
            if (newsArticle.NewsTitle.Length > 200) return false;
            if (newsArticle.NewsContent != null && newsArticle.NewsContent.Length > 4000) return false;
            if (newsArticle.CategoryId <= 0) return false;
            if (newsArticle.CreatedById <= 0) return false;

            // Check if category exists
            return await _categoryRepository.ExistsAsync(newsArticle.CategoryId);
        }

        public async Task<string> GenerateNewsArticleIdAsync()
        {
            string newId;
            do
            {
                newId = "NEWS" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(100, 999);
            }
            while (await _newsArticleRepository.ExistsAsync(newId));

            return newId;
        }
    }
}
