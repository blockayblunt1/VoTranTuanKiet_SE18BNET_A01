using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUNewsManagement.Models
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }

        [Required]
        [StringLength(30)]
        public string TagName { get; set; } = string.Empty;

        [StringLength(4000)]
        public string? Note { get; set; }

        [StringLength(20)]
        public string? NewsArticleId { get; set; }

        // Navigation property
        [ForeignKey("NewsArticleId")]
        public virtual NewsArticle? NewsArticle { get; set; }
    }
}
