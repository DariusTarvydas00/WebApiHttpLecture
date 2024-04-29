using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.DataAccessLayer.Models
{
    [Table("Users")]
    public class UserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }


        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }


        [StringLength(50)]
        public string Role { get; set; }


        [StringLength(255)]
        public string Password { get; set; }

        public virtual ICollection<ReviewModel> Reviews { get; set; } = new HashSet<ReviewModel>();
    }
}
