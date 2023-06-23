
namespace BusinessLogicLayer.ViewModels
{
    /// <summary>
    /// Data Transfer Object for Index of Fuel cards
    /// </summary>
    public class FuelCardListViewModel
    {
        public int FuelCardID { get; internal set; }
        public string CardNumber { get; internal set; }
        public DateTime ExpirationDate { get; internal set; }
        public bool Isdisabled { get; internal set; }
        public string Pincode { get; internal set; }

        public string? Driver { get; internal set; }
        public int? DriverID { get; set; }

        public string FuelType { get; internal set; }
        public List<int> FuelTypeID { get; set; }

    }
}
