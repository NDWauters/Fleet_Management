using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.ViewModels.Driver
{
    public class AddressModel
    {
        //public int AddressID { get; set; }

        [Required] 
        public string Street { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public int Zipcode { get; set; }

        [Required]
        public string Place { get; set; }
    }
}