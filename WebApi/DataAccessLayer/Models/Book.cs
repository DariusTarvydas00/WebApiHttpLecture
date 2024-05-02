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
        [StringLength(300)]
        public string Title { get; set; }

        [Required]
        [StringLength(150)]
        public string Author { get; set; }

        [Required]
        public int PublicationYear { get; set; }

        [StringLength(150)]
        public string? Publisher { get; set; }

        [StringLength(150)]
        public string? Image_URL_S { get; set; }

        [StringLength(150)]
        public string? Image_URL_M { get; set; }

        [StringLength(150)]
        public string? Image_URL_L { get; set; }

        [JsonIgnore]
        public virtual ICollection<Review>? Reviews { get; set; } = new HashSet<Review>();
    }
}
