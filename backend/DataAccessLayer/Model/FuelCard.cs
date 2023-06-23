using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Model
{
    public class FuelCard
    {
        public FuelCard()
        {
            this.FuelCardFuelType = new HashSet<FuelCardFuelType>();
        }

        [Key]
        public int FuelCardID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(10)")]
        [MaxLength(10)]
        public string CardNumber { get; set; }

        [Required]
        [Column(TypeName = "datetime2(7)")]
        public System.DateTime ExpirationDate { get; set; }

        [Column(TypeName = "nvarchar(4)")]
        public string Pincode { get; set; }

        public int? DriverID { get; set; }

        [Required]
        public bool IsDisabled { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual Driver Driver { get; set; }
        public virtual ICollection<FuelCardFuelType> FuelCardFuelType { get; set; }
    }
}
