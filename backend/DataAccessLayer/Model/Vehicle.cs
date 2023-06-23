using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Model
{
    public class Vehicle
    {
        public Vehicle() { }

        [Key]
        public int VehicleID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(150)")]
        [MaxLength(150)]
        public string Model { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(7)")]
        [MaxLength(7)]
        public string LicensePlate { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(17)")]
        [MaxLength(17)]
        public string Vin { get; set; }

        [Required]
        public int BrandID { get; set; }

        [Required]
        public int VehicleTypeID { get; set; }

        [Required]
        public int FuelTypeID { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [MaxLength(150)]
        public string Color { get; set; }

        public int? AmountDoors { get; set; }

        public int? DriverID { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual Driver Driver { get; set; }
        public virtual FuelType FuelType { get; set; }
        public virtual VehicleType VehicleType { get; set; }
        public virtual Brand Brand { get; set; }
    }
}
