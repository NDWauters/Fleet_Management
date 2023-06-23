using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.ViewModels.Vehicle
{
    /// <summary>
    /// Data Transfer Object for Create/Edit
    /// </summary>
    public class VehicleModel
    {
        public int VehicleID { get; set; }

        [Required]
        [StringLength(150)]
        public string Model { get; set; }

        [Required]
        public int BrandID { get; set; }

        [Required]
        public int VehicleTypeID { get; set; }

        [Required]
        public int FuelTypeID { get; set; }

        public int? DriverID { get; set; }

        [Required]
        [StringLength(17)]
        public string Vin { get; set; }

        [Required]
        [StringLength(7)]
        public string LicensePlate { get; set; }

        [StringLength(150)]
        public string Color { get; set; }

        public int? AmountDoors { get; set; }

        public SelectList? BrandSelectList { get; set; }
        public SelectList? FuelTypeSelectList { get; set; }
        public SelectList? VehicleTypeSelectList { get; set; }
        public SelectList? DriverSelectList { get; set; }
    }
}
