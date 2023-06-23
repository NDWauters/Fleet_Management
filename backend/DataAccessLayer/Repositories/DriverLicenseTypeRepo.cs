using DataAccessLayer.Interfaces;
using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    /// <summary>
    /// class responsible for taking care of communication with db
    /// </summary>

    public class DriverLicenseTypeRepo : IDriverLicenseTypeRepo
    {
        private readonly FleetContext _db;

        public DriverLicenseTypeRepo(FleetContext db)
        {
            _db = db;
        }

        #region INDEX-GET ALL
        /// <summary>
        /// Gets all driverLicenseTypes that are active from DB
        /// </summary>
        /// <returns>List of DriverLicenseType objects</returns>
        public List<DriverLicenseType> GetAll()
        {
            return _db.DriverLicenseType
                .Include("DriverDriverLicenseType.Driver")
                .Where(x => x.IsActive)
                .ToList();
        }
        #endregion

        #region DETAILS
        /// <summary>
        /// Gets data of specific driverLicenseType from DB.
        /// </summary>
        /// <param name="id">id of specific driverLicenseType</param>
        /// <returns>DriverLicenseType object</returns>
        public DriverLicenseType Get(int id)
        {
            return _db.DriverLicenseType
                .Include("Driver")
                .FirstOrDefault(x => x.DriverLicenseTypeID == id);
        }
        #endregion

        #region REMOVE
        /// <summary>
        /// Handles soft deleting of a specific driverLicenseType
        /// </summary>
        /// <param name="id">id of target driverLicenseType</param>
        public void Delete(int id)
        {
            var driverLicenseType = _db.DriverLicenseType.First(x => x.DriverLicenseTypeID == id);
            //
            driverLicenseType.IsActive = false;
            //
            _db.SaveChanges();
        }
        #endregion

        #region CREATE/ADD
        /// <summary>
        /// Adds a new driverLicenseType to db.
        /// </summary>
        /// <param name="entity">DriverLicenseType object</param>
        /// <returns>id of new driverLicenseType</returns>
        public int Add(DriverLicenseType entity)
        {
            _db.DriverLicenseType.Add(entity);
            _db.SaveChanges();

            return entity.DriverLicenseTypeID;
        }
        #endregion

        #region EDIT/UPDATE
        /// <summary>
        /// Update an driverLicenseType.
        /// </summary>
        /// <param name="entity">DriverLicenseType object</param>
        /// <returns>id of driverLicenseType</returns>
        public int Update(DriverLicenseType entity)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region HELPERS
        /// <summary>
        /// Function for deleting driverlicensetypes from driver (linking table) 
        /// </summary>
        /// <param name="driverId">id of driver</param>
        /// <param name="driverlicenseTypeId">id of license type</param>
        public void DeleteDriverDriverLicenseType(int driverId, int driverlicenseTypeId)
        {
            var licenseType = _db.DriverDriverLicenseType
                .FirstOrDefault(x =>
                    x.DriverID == driverId &&
                    x.DriverLicenseTypeID == driverlicenseTypeId);

            if (licenseType != null)
            {
                _db.DriverDriverLicenseType.Remove(licenseType);
            }
        }
        #endregion

    }
}
