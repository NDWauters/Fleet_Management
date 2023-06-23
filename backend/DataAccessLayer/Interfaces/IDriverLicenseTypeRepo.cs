using DataAccessLayer.Model;

namespace DataAccessLayer.Interfaces
{
    public interface IDriverLicenseTypeRepo : IRepository<DriverLicenseType>
    {

        #region HELPERS
        /// <summary>
        /// Function for deleting driverlicensetypes from driver (linking table) 
        /// </summary>
        /// <param name="driverId">id of driver</param>
        /// <param name="driverlicenseTypeId">id of license type</param>
        void DeleteDriverDriverLicenseType(int driverId, int driverLicenseTypeId);
        #endregion

    }
}
