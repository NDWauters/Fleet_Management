using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.ViewModels.FuelCard
{
    /// <summary>
    /// Data Transfer Object for Create/Edit fuel card
    /// </summary>
    public class FuelCardModel
    {
        [Required]
        public int FuelCardID { get; set; }
        [Required]
        [StringLength(10)]
        public string CardNumber { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }

        [MinLength(4)]
        [StringLength(4)]
        public string Pincode { get; set; }
        public int? DriverID { get; set; }
        [Required]
        public bool IsDisabled { get; set; }

        [Required]
        public List<int> FuelTypeID { get; set; }

        public MultiSelectList? FuelTypeSelectList { get; set; }
        public SelectList? DriverSelectList { get; set; }

    }
}
