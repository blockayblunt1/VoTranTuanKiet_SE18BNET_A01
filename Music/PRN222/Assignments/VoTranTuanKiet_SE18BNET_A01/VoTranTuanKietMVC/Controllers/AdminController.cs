using Microsoft.AspNetCore.Mvc;
using FUNewsManagement.BusinessLogic.Services;
using FUNewsManagement.Models;

namespace VoTranTuanKietMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly ISystemAccountService _accountService;
        private readonly INewsArticleService _newsArticleService;

        public AdminController(ISystemAccountService accountService, INewsArticleService newsArticleService)
        {
            _accountService = accountService;
            _newsArticleService = newsArticleService;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var accounts = await _accountService.GetAllAccountsAsync();
            return View(accounts);
        }

        [HttpGet]
        public async Task<IActionResult> ManageAccounts(string searchTerm = "")
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            IEnumerable<SystemAccount> accounts;
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                accounts = await _accountService.GetAllAccountsAsync();
            }
            else
            {
                accounts = await _accountService.SearchAccountsAsync(searchTerm);
            }

            ViewBag.SearchTerm = searchTerm;
            return View(accounts);
        }

        [HttpGet]
        public IActionResult CreateAccount()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            return PartialView("_CreateAccountModal", new SystemAccount());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(SystemAccount account)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            try
            {
                if (ModelState.IsValid)
                {
                    await _accountService.CreateAccountAsync(account);
                    TempData["SuccessMessage"] = "Account created successfully.";
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return PartialView("_CreateAccountModal", account);
        }

        [HttpGet]
        public async Task<IActionResult> EditAccount(short id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null) return NotFound();

            return PartialView("_EditAccountModal", account);
        }

        [HttpPost]
        public async Task<IActionResult> EditAccount(SystemAccount account)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            try
            {
                if (ModelState.IsValid)
                {
                    await _accountService.UpdateAccountAsync(account);
                    TempData["SuccessMessage"] = "Account updated successfully.";
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return PartialView("_EditAccountModal", account);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(short id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            try
            {
                var success = await _accountService.DeleteAccountAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Account deleted successfully.";
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Account not found." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> StatisticsReport(DateTime? startDate, DateTime? endDate)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            if (startDate.HasValue && endDate.HasValue)
            {
                var newsArticles = await _newsArticleService.GetNewsArticlesByDateRangeAsync(startDate.Value, endDate.Value);
                ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
                ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");
                return View(newsArticles);
            }

            ViewBag.StartDate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            ViewBag.EndDate = DateTime.Now.ToString("yyyy-MM-dd");
            return View(new List<NewsArticle>());
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }
    }
}
