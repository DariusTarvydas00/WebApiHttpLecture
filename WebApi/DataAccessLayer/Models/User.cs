﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.DataAccessLayer.Models
{

    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        [StringLength(50)]
        public string? Role { get; set; }
        public string? Location { get; set; }

        [StringLength(3)]
        public string? Age { get; set; }

        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
    }
}

