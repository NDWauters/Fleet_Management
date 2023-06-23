using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Model
{
    public class DriverLicenseType
    {
        public DriverLicenseType()
        {
            this.DriverDriverLicenseType = new HashSet<DriverDriverLicenseType>();
        }

        [Key]
        public int DriverLicenseTypeID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(150)")]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual ICollection<DriverDriverLicenseType> DriverDriverLicenseType { get; set; }
    }
}
