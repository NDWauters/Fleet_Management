using DataAccessLayer.Interfaces;
using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    /// <summary>
    /// class responsible for taking care of communication with db
    /// </summary>
    public class DriverRepo : IDriverRepo
    {
        private readonly FleetContext _db;

        public DriverRepo(FleetContext db)
        {
            _db = db;
        }

        #region INDEX-GET ALL
        /// <summary>
        /// Gets all drivers that are active from DB
        /// </summary>
        /// <returns>List of Driver objects</returns>
        public List<Driver> GetAll()
        {
            return _db.Driver
                .Include("Address")
                .Include("Vehicle")
                .Include("FuelCard")
                .Include("Vehicle.Brand")
                .Where(x => x.IsActive)
                .ToList();
        }
        #endregion

        #region DETAILS
        /// <summary>
        /// Gets data of specific driver from DB.
        /// </summary>
        /// <param name="id">id of specific driver</param>
        /// <returns>Driver object</returns>
        public Driver Get(int id)
        {
            return _db.Driver
                .Include("Address")
                .Include("Vehicle")
                .Include("Vehicle.Brand")
                .Include("FuelCard")
                .Include("DriverDriverLicenseType.DriverLicenseType")
                .FirstOrDefault(x => x.DriverID == id);
        }
        #endregion

        #region REMOVE
        /// <summary>
        /// Handles soft deleting of a specific driver
        /// </summary>
        /// <param name="id">id of target driver</param>
        public void Delete(int id)
        {
            //driver
            var driver = _db.Driver
                .First(x => x.DriverID == id);

            driver.IsActive = false;
            driver.FuelCardID = null;
            driver.VehicleID = null;

            // reset DriverID in FuelCard
            if (driver.FuelCard != null)
            {
                driver.FuelCard.DriverID = null;
            }

            // reset DriverID in Vehicle
            if (driver.Vehicle != null)
            {
                driver.Vehicle.DriverID = null;
            }

            //address
            var address = _db.Address
                .First(x => x.AddressID == driver.AddressID);

            address.IsActive = false;

            _db.SaveChanges();
        }
        #endregion

        #region CREATE/ADD
        /// <summary>
        /// Adds a new driver to db.
        /// </summary>
        /// <param name="entity">Driver object</param>
        /// <returns>id of new driver</returns>
        public int Add(Driver entity)
        {
            _db.Driver.Add(entity);
            _db.SaveChanges();

            var newID = entity.DriverID;
            Console.WriteLine(newID);

            return newID;   
        }
        #endregion

        #region EDIT/UPDATE
        /// <summary>
        /// Update van driver.
        /// </summary>
        /// <param name="entity">Driver object</param>
        /// <returns>id of driver</returns>
        public int Update(Driver entity)
        {
            _db.SaveChanges();

            return entity.DriverID;
        }
        #endregion

        #region HELPERS
        /// <summary>
        /// Checks for NationalInsuranceNr
        /// </summary>
        /// <param name="NationalInsuranceNr">NationalInsuranceNr to check</param>
        /// <returns>bool</returns>
        public bool DoesNationalInsuranceNrExists(string nationalInsuranceNr)
        {
            var allNationalInsuranceNrs = _db.Driver
                .Where(x => x.IsActive)
                .Select(x => x.NationalInsuranceNr)
                .ToList();

            return allNationalInsuranceNrs
                .Any(x => x.Trim() == nationalInsuranceNr.Trim());
        }

        public bool DoesNationalInsuranceNrIsValid(string nationalInsuranceNr)
        {
            if (nationalInsuranceNr.Trim().Length == 11)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region GetOnlyAvailableDrivers
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Driver> GetOnlyAvailableDrivers(int? id = null)
        {
            return _db.Driver
                .Where(x => (x.IsActive == true))
                .Where(x => x.FuelCardID == null || (id != null && x.DriverID == id))
                .ToList();
        }
        #endregion

    }
}
