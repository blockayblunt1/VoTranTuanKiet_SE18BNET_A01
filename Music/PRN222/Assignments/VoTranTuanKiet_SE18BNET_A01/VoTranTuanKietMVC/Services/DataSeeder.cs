using FUNewsManagement.DataAccess;
using FUNewsManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace VoTranTuanKietMVC.Services
{
    public class DataSeeder
    {
        private readonly FUNewsManagementDbContext _context;

        public DataSeeder(FUNewsManagementDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Ensure database is created
            await _context.Database.EnsureCreatedAsync();

            // Seed Categories
            if (!await _context.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category
                    {
                        CategoryName = "University News",
                        CategoryDesciption = "Official university announcements and news",
                        IsActive = true
                    },
                    new Category
                    {
                        CategoryName = "Academic",
                        CategoryDesciption = "Academic programs, courses, and educational content",
                        IsActive = true
                    },
                    new Category
                    {
                        CategoryName = "Student Life",
                        CategoryDesciption = "Student activities, events, and campus life",
                        IsActive = true
                    },
                    new Category
                    {
                        CategoryName = "Research",
                        CategoryDesciption = "Research projects, publications, and innovations",
                        IsActive = true
                    },
                    new Category
                    {
                        CategoryName = "Events",
                        CategoryDesciption = "Upcoming events, seminars, and workshops",
                        IsActive = true
                    }
                };

                await _context.Categories.AddRangeAsync(categories);
                await _context.SaveChangesAsync();
            }

            // Seed System Accounts
            if (!await _context.SystemAccounts.AnyAsync())
            {
                var accounts = new List<SystemAccount>
                {
                    new SystemAccount
                    {
                        AccountName = "John Smith",
                        AccountEmail = "john.smith@fu.edu.vn",
                        AccountRole = 1, // Staff
                        AccountPassword = "staff123"
                    },
                    new SystemAccount
                    {
                        AccountName = "Jane Doe",
                        AccountEmail = "jane.doe@fu.edu.vn",
                        AccountRole = 1, // Staff
                        AccountPassword = "staff123"
                    },
                    new SystemAccount
                    {
                        AccountName = "Dr. Michael Johnson",
                        AccountEmail = "michael.johnson@fu.edu.vn",
                        AccountRole = 2, // Lecturer
                        AccountPassword = "lecturer123"
                    },
                    new SystemAccount
                    {
                        AccountName = "Prof. Sarah Wilson",
                        AccountEmail = "sarah.wilson@fu.edu.vn",
                        AccountRole = 2, // Lecturer
                        AccountPassword = "lecturer123"
                    }
                };

                await _context.SystemAccounts.AddRangeAsync(accounts);
                await _context.SaveChangesAsync();
            }

            // Seed News Articles
            if (!await _context.NewsArticles.AnyAsync())
            {
                var categories = await _context.Categories.ToListAsync();
                var accounts = await _context.SystemAccounts.Where(a => a.AccountRole == 1).ToListAsync(); // Staff only

                if (categories.Any() && accounts.Any())
                {
                    var newsArticles = new List<NewsArticle>
                    {
                        new NewsArticle
                        {
                            NewsArticleId = "NEWS20250101001",
                            NewsTitle = "Welcome to the New Academic Year 2025",
                            NewsContent = "We are excited to welcome all students, faculty, and staff to the new academic year 2025. This year brings new opportunities, challenges, and exciting developments in our educational programs. We look forward to a successful year ahead with innovative teaching methods and enhanced learning experiences.",
                            CategoryId = categories.First(c => c.CategoryName == "University News").CategoryId,
                            NewsStatus = true,
                            CreatedById = accounts.First().AccountId,
                            CreatedDate = DateTime.Now.AddDays(-10)
                        },
                        new NewsArticle
                        {
                            NewsArticleId = "NEWS20250102001",
                            NewsTitle = "New Computer Science Program Launched",
                            NewsContent = "FU University is proud to announce the launch of our new Computer Science program with specializations in Artificial Intelligence, Cybersecurity, and Software Engineering. The program is designed to meet industry demands and prepare students for the digital future.",
                            CategoryId = categories.First(c => c.CategoryName == "Academic").CategoryId,
                            NewsStatus = true,
                            CreatedById = accounts.First().AccountId,
                            CreatedDate = DateTime.Now.AddDays(-8)
                        },
                        new NewsArticle
                        {
                            NewsArticleId = "NEWS20250103001",
                            NewsTitle = "Annual Tech Conference 2025",
                            NewsContent = "Join us for the Annual Tech Conference 2025 featuring keynote speakers from leading technology companies. The event will showcase the latest innovations in technology and provide networking opportunities for students and professionals.",
                            CategoryId = categories.First(c => c.CategoryName == "Events").CategoryId,
                            NewsStatus = true,
                            CreatedById = accounts.Skip(1).First().AccountId,
                            CreatedDate = DateTime.Now.AddDays(-5)
                        },
                        new NewsArticle
                        {
                            NewsArticleId = "NEWS20250104001",
                            NewsTitle = "Student Research Excellence Awards",
                            NewsContent = "Congratulations to our outstanding students who received the Research Excellence Awards for their innovative projects in various fields including AI, biotechnology, and sustainable engineering.",
                            CategoryId = categories.First(c => c.CategoryName == "Research").CategoryId,
                            NewsStatus = true,
                            CreatedById = accounts.First().AccountId,
                            CreatedDate = DateTime.Now.AddDays(-3)
                        },
                        new NewsArticle
                        {
                            NewsArticleId = "NEWS20250105001",
                            NewsTitle = "Campus Life: New Student Activities Center",
                            NewsContent = "The new Student Activities Center is now open! Featuring modern facilities for sports, recreation, and social activities. Students can enjoy the new gym, study areas, and event spaces.",
                            CategoryId = categories.First(c => c.CategoryName == "Student Life").CategoryId,
                            NewsStatus = true,
                            CreatedById = accounts.Skip(1).First().AccountId,
                            CreatedDate = DateTime.Now.AddDays(-1)
                        }
                    };

                    await _context.NewsArticles.AddRangeAsync(newsArticles);
                    await _context.SaveChangesAsync();

                    // Seed Tags
                    var tags = new List<Tag>
                    {
                        new Tag { TagName = "academic-year", NewsArticleId = "NEWS20250101001" },
                        new Tag { TagName = "welcome", NewsArticleId = "NEWS20250101001" },
                        new Tag { TagName = "computer-science", NewsArticleId = "NEWS20250102001" },
                        new Tag { TagName = "AI", NewsArticleId = "NEWS20250102001" },
                        new Tag { TagName = "cybersecurity", NewsArticleId = "NEWS20250102001" },
                        new Tag { TagName = "conference", NewsArticleId = "NEWS20250103001" },
                        new Tag { TagName = "technology", NewsArticleId = "NEWS20250103001" },
                        new Tag { TagName = "networking", NewsArticleId = "NEWS20250103001" },
                        new Tag { TagName = "research", NewsArticleId = "NEWS20250104001" },
                        new Tag { TagName = "awards", NewsArticleId = "NEWS20250104001" },
                        new Tag { TagName = "innovation", NewsArticleId = "NEWS20250104001" },
                        new Tag { TagName = "campus-life", NewsArticleId = "NEWS20250105001" },
                        new Tag { TagName = "facilities", NewsArticleId = "NEWS20250105001" },
                        new Tag { TagName = "student-center", NewsArticleId = "NEWS20250105001" }
                    };

                    await _context.Tags.AddRangeAsync(tags);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
