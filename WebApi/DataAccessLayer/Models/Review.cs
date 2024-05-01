using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApi.DataAccessLayer.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }  // Foreign key for User

        [Required]
        public int BookId { get; set; }  // Foreign key for Book

        [Required]
        [StringLength(1000)]
        public string ReviewText { get; set; }

        [Required]
        [Range(0, 10)]
        public int Rating { get; set; }  // Assuming a rating out of 5

        // Navigation properties to link to the user and the book
        [ForeignKey("UserId")]
        [JsonIgnore]
        public virtual User? User { get; set; }

        [ForeignKey("BookId")]
        [JsonIgnore]
        public virtual Book? Book { get; set; }
    }
}
