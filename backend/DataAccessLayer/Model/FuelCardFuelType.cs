namespace DataAccessLayer.Model
{
    public class FuelCardFuelType
    {
        public int FuelCardID { get; set; }
        public int FuelTypeID { get; set; }

        public FuelCard FuelCard { get; set; }
        public FuelType FuelType { get; set; }
    }
}
