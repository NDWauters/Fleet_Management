using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Model
{
    public class Driver
    {
        public Driver()
        {
            this.DriverDriverLicenseType = new HashSet<DriverDriverLicenseType>();
        }

        [Key]
        public int DriverID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(150)")]
        [MaxLength(150)]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(150)")]
        [MaxLength(150)]
        public string FirstName { get; set; }

        [Required]
        [Column(TypeName = "datetime2(7)")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(15)")]
        [MaxLength(11)]
        public string NationalInsuranceNr { get; set; }

        public int? VehicleID { get; set; }

        public int? FuelCardID { get; set; }

        [Required]
        public int AddressID { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual FuelCard FuelCard { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<DriverDriverLicenseType> DriverDriverLicenseType { get; set; }
    }
}
