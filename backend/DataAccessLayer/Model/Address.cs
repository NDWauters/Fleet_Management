using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Model
{
    public class Address
    {
        public Address()
        {

        }

        [Key]
        public int AddressID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(200)")]
        [MaxLength(200)]
        public string Street { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(200)")]
        [MaxLength(200)]
        public string Place { get; set; }

        [Required]
        public int Zipcode { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual Driver Driver { get; set; }
    }
}
