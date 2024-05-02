using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApi.DataAccessLayer.Models
{

    [Table("Users")]
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        public byte[]? PasswordHash { get; set; }

        public byte[]? PasswordSalt { get; set; }

        [StringLength(50)]
        public string? Role { get; set; }

        [StringLength(150)]
        public string? Location { get; set; }

        public int? Age { get; set; }

        [JsonIgnore]
        public virtual ICollection<Review>? Reviews { get; set; } = new HashSet<Review>();
    }
}

