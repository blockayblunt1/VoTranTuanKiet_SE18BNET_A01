using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUNewsManagement.Models
{
    public class SystemAccount
    {
        [Key]
        public short AccountId { get; set; }

        [Required]
        [StringLength(120)]
        public string AccountName { get; set; } = string.Empty;

        [Required]
        [StringLength(120)]
        [EmailAddress]
        public string AccountEmail { get; set; } = string.Empty;

        [Required]
        public short AccountRole { get; set; } // 1: Staff, 2: Lecturer

        [Required]
        [StringLength(120)]
        public string AccountPassword { get; set; } = string.Empty;

        // Navigation property
        public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
    }
}
