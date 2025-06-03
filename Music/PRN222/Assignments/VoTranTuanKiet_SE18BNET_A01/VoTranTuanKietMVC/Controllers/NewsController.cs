using Microsoft.AspNetCore.Mvc;
using FUNewsManagement.BusinessLogic.Services;
using FUNewsManagement.Models;

namespace VoTranTuanKietMVC.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;

        public NewsController(INewsArticleService newsArticleService, ICategoryService categoryService)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string searchTerm = "", short? categoryId = null)
        {
            IEnumerable<NewsArticle> newsArticles;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var allArticles = await _newsArticleService.SearchNewsArticlesAsync(searchTerm);
                newsArticles = allArticles.Where(n => n.NewsStatus == true);
            }
            else if (categoryId.HasValue)
            {
                var allArticles = await _newsArticleService.GetNewsArticlesByCategoryAsync(categoryId.Value);
                newsArticles = allArticles.Where(n => n.NewsStatus == true);
            }
            else
            {
                newsArticles = await _newsArticleService.GetActiveNewsArticlesAsync();
            }

            ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.IsLoggedIn = !string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail"));
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");

            return View(newsArticles);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var newsArticle = await _newsArticleService.GetNewsArticleByIdAsync(id);
            if (newsArticle == null || newsArticle.NewsStatus != true) return NotFound();

            ViewBag.IsLoggedIn = !string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail"));
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");

            return View(newsArticle);
        }

        [HttpGet]
        public async Task<IActionResult> ByCategory(short id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null || category.IsActive != true) return NotFound();

            var newsArticles = await _newsArticleService.GetNewsArticlesByCategoryAsync(id);
            var activeArticles = newsArticles.Where(n => n.NewsStatus == true);

            ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
            ViewBag.SelectedCategory = category;
            ViewBag.IsLoggedIn = !string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail"));
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");

            return View("Index", activeArticles);
        }
    }
}
