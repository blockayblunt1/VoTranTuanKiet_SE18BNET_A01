using Microsoft.AspNetCore.Mvc;
using FUNewsManagement.BusinessLogic.Services;
using FUNewsManagement.Models;

namespace VoTranTuanKietMVC.Controllers
{
    public class StaffController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly INewsArticleService _newsArticleService;
        private readonly ISystemAccountService _accountService;

        public StaffController(
            ICategoryService categoryService, 
            INewsArticleService newsArticleService,
            ISystemAccountService accountService)
        {
            _categoryService = categoryService;
            _newsArticleService = newsArticleService;
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            var userId = GetCurrentUserId();
            var newsArticles = await _newsArticleService.GetNewsArticlesByCreatedByAsync(userId);
            return View(newsArticles);
        }

        [HttpGet]
        public async Task<IActionResult> ManageCategories(string searchTerm = "")
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            IEnumerable<Category> categories;
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                categories = await _categoryService.GetAllCategoriesAsync();
            }
            else
            {
                categories = await _categoryService.SearchCategoriesAsync(searchTerm);
            }

            ViewBag.SearchTerm = searchTerm;
            return View(categories);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");
            return PartialView("_CreateCategoryModal", new Category());
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            try
            {
                if (ModelState.IsValid)
                {
                    await _categoryService.CreateCategoryAsync(category);
                    TempData["SuccessMessage"] = "Category created successfully.";
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return PartialView("_CreateCategoryModal", category);
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(short id)
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();

            return PartialView("_EditCategoryModal", category);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(Category category)
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            try
            {
                if (ModelState.IsValid)
                {
                    await _categoryService.UpdateCategoryAsync(category);
                    TempData["SuccessMessage"] = "Category updated successfully.";
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return PartialView("_EditCategoryModal", category);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(short id)
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            try
            {
                var canDelete = await _categoryService.CanDeleteCategoryAsync(id);
                if (!canDelete)
                {
                    return Json(new { success = false, message = "Cannot delete category that has associated news articles." });
                }

                var success = await _categoryService.DeleteCategoryAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Category deleted successfully.";
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Category not found." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ManageNewsArticles(string searchTerm = "")
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            var userId = GetCurrentUserId();
            IEnumerable<NewsArticle> newsArticles;
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                newsArticles = await _newsArticleService.GetNewsArticlesByCreatedByAsync(userId);
            }
            else
            {
                var allArticles = await _newsArticleService.SearchNewsArticlesAsync(searchTerm);
                newsArticles = allArticles.Where(n => n.CreatedById == userId);
            }

            ViewBag.SearchTerm = searchTerm;
            return View(newsArticles);
        }

        [HttpGet]
        public async Task<IActionResult> CreateNewsArticle()
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");
            
            ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
            return PartialView("_CreateNewsArticleModal", new NewsArticle());
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewsArticle(NewsArticle newsArticle, string tags)
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            try
            {
                newsArticle.CreatedById = GetCurrentUserId();
                var tagList = string.IsNullOrWhiteSpace(tags) ? new List<string>() : 
                             tags.Split(',').Select(t => t.Trim()).Where(t => !string.IsNullOrWhiteSpace(t)).ToList();

                if (ModelState.IsValid)
                {
                    await _newsArticleService.CreateNewsArticleAsync(newsArticle, tagList);
                    TempData["SuccessMessage"] = "News article created successfully.";
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
            return PartialView("_CreateNewsArticleModal", newsArticle);
        }

        [HttpGet]
        public async Task<IActionResult> EditNewsArticle(string id)
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            var newsArticle = await _newsArticleService.GetNewsArticleByIdAsync(id);
            if (newsArticle == null || newsArticle.CreatedById != GetCurrentUserId()) 
                return NotFound();

            ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
            ViewBag.Tags = string.Join(", ", newsArticle.Tags.Select(t => t.TagName));
            return PartialView("_EditNewsArticleModal", newsArticle);
        }

        [HttpPost]
        public async Task<IActionResult> EditNewsArticle(NewsArticle newsArticle, string tags)
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            try
            {
                var existingArticle = await _newsArticleService.GetNewsArticleByIdAsync(newsArticle.NewsArticleId);
                if (existingArticle == null || existingArticle.CreatedById != GetCurrentUserId())
                    return NotFound();

                var tagList = string.IsNullOrWhiteSpace(tags) ? new List<string>() : 
                             tags.Split(',').Select(t => t.Trim()).Where(t => !string.IsNullOrWhiteSpace(t)).ToList();

                if (ModelState.IsValid)
                {
                    await _newsArticleService.UpdateNewsArticleAsync(newsArticle, tagList);
                    TempData["SuccessMessage"] = "News article updated successfully.";
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            ViewBag.Categories = await _categoryService.GetActiveCategoriesAsync();
            return PartialView("_EditNewsArticleModal", newsArticle);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNewsArticle(string id)
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            try
            {
                var newsArticle = await _newsArticleService.GetNewsArticleByIdAsync(id);
                if (newsArticle == null || newsArticle.CreatedById != GetCurrentUserId())
                    return Json(new { success = false, message = "News article not found or access denied." });

                var success = await _newsArticleService.DeleteNewsArticleAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "News article deleted successfully.";
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "News article not found." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            var userId = GetCurrentUserId();
            var account = await _accountService.GetAccountByIdAsync(userId);
            if (account == null) return NotFound();

            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(SystemAccount account)
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            try
            {
                var userId = GetCurrentUserId();
                if (account.AccountId != userId) return Forbid();

                if (ModelState.IsValid)
                {
                    await _accountService.UpdateAccountAsync(account);
                    HttpContext.Session.SetString("UserName", account.AccountName);
                    TempData["SuccessMessage"] = "Profile updated successfully.";
                    return RedirectToAction("Profile");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View("Profile", account);
        }

        private bool IsStaff()
        {
            return HttpContext.Session.GetString("UserRole") == "Staff";
        }

        private short GetCurrentUserId()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            return short.TryParse(userIdString, out var userId) ? userId : (short)0;
        }
    }
}
