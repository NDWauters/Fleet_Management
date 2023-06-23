using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataAccessLayer.Model
{
    public  class User
    {
        public User() { }

        [Key]
        public int UserID { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        [MaxLength(200)]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        [MaxLength(200)]
        public string LastName { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        [MaxLength(200)]
        [Required]
        public string UserName { get; set; }

        [JsonIgnore]
        [Required]
        public string PasswordHash { get; set; }
    }
}
