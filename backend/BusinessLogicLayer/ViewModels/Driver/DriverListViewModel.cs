namespace BusinessLogicLayer.ViewModels
{
    /// <summary>
    /// Data Transfer Object for Index of Drivers
    /// </summary>
    public class DriverListViewModel
    {
        public int DriverID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Name => $"{LastName} {FirstName}";
        public DateTime DateOfBirth { get; set; }
        public string NationalInsuranceNr { get; set; }

        public int? AddressID { get; set; }
        //public string? Address { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public int Zipcode { get; set; }
        public string Place { get; set; }
        
       //public List<int> DriverLicenseTypeID { get; set; }
       //public List<string> DriverLicenseTypes { get; set; }
       
       public int? VehicleID { get; set; }
       //public string? Vehicle { get; set; }
       public string? Brand { get; set; }
       public string? Model { get; set; }
       public string? VehicleSoort => $"{Brand} - {Model}";
       public string? LicensePlate { get; set; }

       public int? FuelCardID { get; set; }
       //public string? FuelCard { get; set; }
       public string? CardNumber { get; set; }
    }
}
