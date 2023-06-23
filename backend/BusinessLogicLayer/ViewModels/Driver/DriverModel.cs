using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.ViewModels.Driver
{
    /// <summary>
    /// Data Transfer Object for Create/Edit Driver
    /// </summary>
    public class DriverModel
    {
        public int DriverID { get; set; }

        [Required]
        [StringLength(150)]
        public string LastName { get; set; }

        [Required]
        [StringLength(150)]
        public string FirstName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(11)]
        public string NationalInsuranceNr { get; set; }

        public AddressModel Address { get; set; }

        //public int AddressID { get; set; }

        public List<int> DriverLicenseTypeID { get; set; }

        public int? VehicleID { get; set; }

        public int? FuelCardID { get; set; }

        public string? CardNumber { get; set; }

        public MultiSelectList? DriverLicenseTypeSelectList { get; set; }

        public SelectList? FuelCardSelectList { get; set; }

        public SelectList? VehicleSelectList { get; set; }

    }
}
