using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUNewsManagement.Models
{
    public class NewsArticle
    {
        [Key]
        [StringLength(20)]
        public string NewsArticleId { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string NewsTitle { get; set; } = string.Empty;

        [StringLength(4000)]
        public string? NewsContent { get; set; }

        [Required]
        public short CategoryId { get; set; }

        public bool? NewsStatus { get; set; } = true; // 1: Active, 0: Inactive

        [Required]
        public short CreatedById { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; } = null!;

        [ForeignKey("CreatedById")]
        public virtual SystemAccount CreatedBy { get; set; } = null!;

        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
