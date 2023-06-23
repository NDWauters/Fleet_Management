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
    public class VehicleRepo : IVehicleRepo
    {
        private readonly FleetContext _db;

        public VehicleRepo(FleetContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Adds a new vehicle to db.
        /// </summary>
        /// <param name="entity">Vehicle object</param>
        /// <returns>id of new vehicle</returns>
        public int Add(Vehicle entity)
        {
            _db.Vehicle.Add(entity);
            _db.SaveChanges();

            return entity.VehicleID;
        }

        /// <summary>
        /// Handles soft deleting of a specific vehicle
        /// </summary>
        /// <param name="id">id of target vehicle</param>
        public void Delete(int id)
        {
            var vehicle = _db.Vehicle
                .First(x => x.VehicleID == id);

            vehicle.IsActive = false;

            // get driver linked on this vehicle
            var driver = _db.Driver
                .FirstOrDefault(x => x.DriverID == vehicle.DriverID);

            // check for null
            if (driver != null)
            {
                // reset vehicle in driver
                driver.VehicleID = null;
            }

            // to be sure, reset driverID in vehicle
            vehicle.DriverID = null;

            _db.SaveChanges();
        }

        /// <summary>
        /// Gets data of specific vehicle from DB.
        /// </summary>
        /// <param name="id">id of specific vehicle</param>
        /// <returns>Vehicle object</returns>
        public Vehicle Get(int id)
        {
            return _db.Vehicle
                .Include("VehicleType")
                .Include("Brand")
                .Include("FuelType")
                .Include("Driver.FuelCard")
                .FirstOrDefault(x => x.VehicleID == id);
        }

        /// <summary>
        /// Gets all vehicles that are active from DB
        /// </summary>
        /// <returns>List of vehicle objects</returns>
        public List<Vehicle> GetAll()
        {
            return _db.Vehicle
                .Include("VehicleType")
                .Include("Brand")
                .Include("FuelType")
                .Include("Driver")
                .Where(x => x.IsActive)
                .ToList();
        }

        /// <summary>
        /// Handles updating of vehicle in db
        /// </summary>
        /// <param name="entity">vehicle db object</param>
        /// <returns>id of vehicle</returns>
        public int Update(Vehicle entity)
        {
            _db.SaveChanges();

            return entity.VehicleID;
        }

        /// <summary>
        /// Checks if given license plate already exists in db.
        /// </summary>
        /// <param name="licensePlate">License plate to check</param>
        /// <returns>bool</returns>
        public bool DoesLicensePlateExists(string licensePlate)
        {
            var allLicensePlates = _db.Vehicle
                .Where(x => x.IsActive)
                .Select(x => x.LicensePlate)
                .ToList();

            return allLicensePlates
                .Any(x => x.Trim() == licensePlate.Trim());
        }

        /// <summary>
        /// Checks if given VIN already exists in db
        /// </summary>
        /// <param name="vin">VIN to check</param>
        /// <returns>bool</returns>
        public bool DoesVinExists(string vin)
        {
            var allVinNumbers = _db.Vehicle
                .Where(x => x.IsActive)
                .Select(x => x.Vin)
                .ToList();

            return allVinNumbers
                .Any(x => x.Trim() == vin.Trim());
        }

        /// <summary>
        /// Gets only vehicles from DB
        /// </summary>
        /// <returns>List of vehicle objects</returns>
        public List<Vehicle> GetOnlyAvailableVehicles(int? id = null)
        {
            return _db.Vehicle
                .Where(p => p.IsActive == true)
                .Where(x => x.DriverID == null || (id != null && x.VehicleID == id))
                .ToList();
        }
    }
}
