using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement.Models
{
    public class Category
    {
        [Key]
        public short CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(250)]
        public string? CategoryDesciption { get; set; }

        public bool? IsActive { get; set; } = true; // 1: Active, 0: Inactive

        // Navigation property
        public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
    }
}
