using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;


namespace WebApi.DataAccessLayer.Models
{
    [Index(nameof(BookISBN), IsUnique = false)]
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }  // Foreign key for User

        [Required]
        [StringLength(10)]
        public string BookISBN { get; set; }  // Foreign key for Book

        [StringLength(1000)]
        public string? ReviewText { get; set; }

        [Required]
        [Range(0, 10)]
        public int Rating { get; set; }  

        [ForeignKey("UserId")]
        [JsonIgnore]
        public virtual User? User { get; set; }

        [ForeignKey("BookISBN")]
        [JsonIgnore]
        public virtual Book? Book { get; set; }
    }
}
