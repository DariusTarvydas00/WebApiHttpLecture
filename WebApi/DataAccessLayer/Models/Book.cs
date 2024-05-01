using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.DataAccessLayer.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        [Required]
        public int PublicationYear { get; set; }

        [StringLength(13)]
        public string? ISBN { get; set; }

        public string? Publisher { get; set; }

        public string? Image_URL_S { get; set; }

        public string? Image_URL_M { get; set; }

        public string? Image_URL_L { get; set; }

        // Initialize the collection of reviews in the constructor to avoid null references
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
    }
}
