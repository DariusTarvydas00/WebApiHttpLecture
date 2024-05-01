using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApi.DataAccessLayer.Models
{
    public class Book
    {
        [Key]
        [StringLength(10)]
        public string ISBN { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        [Required]
        public int PublicationYear { get; set; }

        public string? Publisher { get; set; }

        public string? Image_URL_S { get; set; }

        public string? Image_URL_M { get; set; }

        public string? Image_URL_L { get; set; }

        [JsonIgnore]
        public virtual ICollection<Review>? Reviews { get; set; } = new HashSet<Review>();
    }
}
