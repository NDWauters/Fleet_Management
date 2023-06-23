namespace BusinessLogicLayer.ViewModels
{
    /// <summary>
    /// Data Transfer Object for Index of Vehicles
    /// </summary>
    public class VehicleListViewModel // to fixx it simple change the name of any of the model
    {
        public int VehicleID { get; internal set; }
        public string Model { get; internal set; }
        public string LicensePlate { get; internal set; }
        public string Vin { get; internal set; }
        public string Brand { get; internal set; }
        public string VehicleType { get; internal set; }
        public string FuelType { get; internal set; }
        public string Color { get; internal set; }
        public int? AmountDoors { get; internal set; }

        public string Driver { get; internal set; }
        public int? DriverID { get; set; }

        public string FuelCard { get; set; }
        public int? FuelCardID { get; set; }
    }
   

}
