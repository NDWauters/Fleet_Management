namespace DataAccessLayer.Model
{
    public class DriverDriverLicenseType
    {
        public int DriverID { get; set; }
        public int DriverLicenseTypeID { get; set; }

        public Driver Driver { get; set; }
        public DriverLicenseType DriverLicenseType { get; set; }
    }
}
