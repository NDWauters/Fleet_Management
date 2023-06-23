using DataAccessLayer.Interfaces;
using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    /// <summary>
    /// Class responsible for taking care of communication with db.
    /// </summary>
    public class FuelCardRepo : IFuelCardRepo
    {
        private readonly FleetContext _db;

        public FuelCardRepo(FleetContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Adds a new fuel card to db.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Id of new fuelcard</returns>
        public int Add(FuelCard entity)
        {
            _db.FuelCard.Add(entity);
            _db.SaveChanges();

            return entity.FuelCardID;
        }

        /// <summary>
        /// Handles soft deleting of a specific fuel card.
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var fuelCard = _db.FuelCard
                .First(x => x.FuelCardID == id);

            fuelCard.IsActive = false;

            // get driver linked on this fuelcard
            var driver = _db.Driver
                .FirstOrDefault(x => x.DriverID == fuelCard.DriverID);

            // check for null
            if (driver != null)
            {
                // reset fulecard in driver
                driver.FuelCardID = null;
            }

            // to be sure, reset driverID in fuelcard
            fuelCard.DriverID = null;

            _db.SaveChanges();
        }

        /// <summary>
        /// Gets data of specific fuel card from DB.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Fuel card object</returns>
        public FuelCard Get(int id)
        {
            return _db.FuelCard
            .Include("Driver")
            .Include("FuelCardFuelType.FuelType")
            .FirstOrDefault(x => x.IsActive && x.FuelCardID == id);
         }

        /// <summary>
        /// Gets all fuel cards that are active from DB.
        /// </summary>
        /// <returns>>List of fuels cards objects</returns>
        public List<FuelCard> GetAll()
        {
            return _db.FuelCard
                .Include("Driver")
                .Include("FuelCardFuelType.FuelType")
                .Where(x => x.IsActive)
                .ToList();
        }

        /// <summary>
        /// Handles updating of fule card in db
        /// </summary>
        /// <param name="entity">fuel card db object</param>
        /// <returns>id of fuel card<returns>
        public int Update(FuelCard entity)
        {
            _db.SaveChanges();
            return entity.FuelCardID;
        }

        /// <summary>
        /// Checks if given cardNumber already exists in db.
        /// </summary>
        /// <param name="CardNumber"></param>
        /// <returns>bool</returns>
        public bool DoesCardNumberExists(string CardNumber)
        {
            var allCardNumbers = _db.FuelCard
                .Where(x => x.IsActive)
                .Select(x => x.CardNumber)
                .ToList();

            return allCardNumbers
                .Any(x => x.Trim() == CardNumber.Trim());
        }

        /// <summary>
        /// Gets only fuelCards from DB.
        /// </summary>
        /// <returns>>List of fuels cards objects</returns>
        public List<FuelCard> GetOnlyAvailableFuelCards(int? id = null)
        {
            return _db.FuelCard
                .Where(x => ((x.IsActive == true) && (x.IsDisabled == false)))
                .Where(x => x.DriverID == null || (id != null && x.FuelCardID == id))
                .ToList();
        }
    }
}
