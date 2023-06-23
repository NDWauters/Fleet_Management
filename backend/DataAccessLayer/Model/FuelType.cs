using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Model
{
    public class FuelType
    {
        public FuelType()
        {
            this.Vehicle = new HashSet<Vehicle>();
            this.FuelCardFuelType = new HashSet<FuelCardFuelType>();
        }

        [Key]
        public int FuelTypeID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(150)")]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual ICollection<Vehicle> Vehicle { get; set; }
        public virtual ICollection<FuelCardFuelType> FuelCardFuelType { get; set; }
    }
}
