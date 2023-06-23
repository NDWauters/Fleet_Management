using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Model
{
    public class Brand
    {
        public Brand()
        {
            this.Vehicle = new HashSet<Vehicle>();
        }

        [Key]
        public int BrandID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(150)")]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual ICollection<Vehicle> Vehicle { get; set; }
    }
}
