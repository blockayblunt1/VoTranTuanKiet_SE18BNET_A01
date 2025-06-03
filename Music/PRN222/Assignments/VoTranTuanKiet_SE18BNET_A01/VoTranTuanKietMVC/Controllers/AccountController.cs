using Microsoft.AspNetCore.Mvc;
using FUNewsManagement.BusinessLogic.Services;
using FUNewsManagement.Models;

namespace VoTranTuanKietMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ISystemAccountService _accountService;
        private readonly IConfiguration _configuration;

        public AccountController(ISystemAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // If already logged in, redirect based on role
            if (HttpContext.Session.GetString("UserEmail") != null)
            {
                return RedirectToAppropriateHome();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.ErrorMessage = "Email and password are required.";
                return View();
            }

            // Check admin account from appsettings.json
            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];

            if (email == adminEmail && password == adminPassword)
            {
                // Admin login
                HttpContext.Session.SetString("UserEmail", email);
                HttpContext.Session.SetString("UserRole", "Admin");
                HttpContext.Session.SetString("UserName", "Administrator");
                return RedirectToAction("Index", "Admin");
            }

            // Check regular user accounts
            var user = await _accountService.AuthenticateAsync(email, password);
            if (user != null)
            {
                HttpContext.Session.SetString("UserEmail", user.AccountEmail);
                HttpContext.Session.SetString("UserRole", user.AccountRole == 1 ? "Staff" : "Lecturer");
                HttpContext.Session.SetString("UserName", user.AccountName);
                HttpContext.Session.SetString("UserId", user.AccountId.ToString());

                return RedirectToAppropriateHome();
            }

            ViewBag.ErrorMessage = "Invalid email or password.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        private IActionResult RedirectToAppropriateHome()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role switch
            {
                "Admin" => RedirectToAction("Index", "Admin"),
                "Staff" => RedirectToAction("Index", "Staff"),
                "Lecturer" => RedirectToAction("Index", "News"),
                _ => RedirectToAction("Login")
            };
        }
    }
}
