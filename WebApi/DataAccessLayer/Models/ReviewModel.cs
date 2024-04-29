using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.DataAccessLayer.Models
{
    public class ReviewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }  // Foreign key for UserModel

        [Required]
        public int BookId { get; set; }  // Foreign key for BookModel

        [Required]
        [StringLength(1000)]
        public string ReviewText { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }  // Assuming a rating out of 5

        // Navigation properties to link to the user and the book
        [ForeignKey("UserId")]
        public virtual UserModel User { get; set; }

        [ForeignKey("BookId")]
        public virtual BookModel Book { get; set; }
    }
}
