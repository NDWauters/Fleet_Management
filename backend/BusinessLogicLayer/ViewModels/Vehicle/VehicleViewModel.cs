namespace BusinessLogicLayer.ViewModels.Vehicle
{
    /// <summary>
    /// Data Transfer Object for Details of vehicle
    /// </summary>
    public class VehicleViewModel
    {
        public int VehicleID { get; set; }

        public string Model { get; set; }

        public string LicensePlate { get; set; }

        public string Vin { get; set; }

        public string Brand { get; set; }

        public string VehicleType { get; set; }

        public string FuelType { get; set; }

        public string Color { get; set; }

        public int? AmountDoors { get; set; }

        public string DriverLastName { get; set; }
        public string DriverFirstName { get; set; }
        public int? DriverID { get; set; }

        public string FuelCard { get; set; }
        public int? FuelCardID { get; set; }
    }
}
