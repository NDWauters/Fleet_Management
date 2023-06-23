using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ViewModels.FuelCard
{
    /// <summary>
    /// Data Transfer Object for Details of fuel card
    /// </summary>
    public class FuelCardViewModel
    {
        public int FuelCardID { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Pincode { get; set; }
        public bool IsDisabled { get; internal set; }

        public string Driver { get; internal set; }
        public int? DriverID { get; set; }
        public string DriverLastName { get; set; }
        public string DriverFirstName { get; set; }
        public string DriverNationalInsuranceNr { get; set; }
        public string DriverLicenseType { get; set; }
        public DateTime? DriverdateOfBirth { get; set; }

        public string FuelType { get; internal set; }
        public int FuelTypeID { get; set; }
    }
}
