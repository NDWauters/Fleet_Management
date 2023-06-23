using DataAccessLayer.Interfaces;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    /// <summary>
    /// Class responsible for taking care of communication with db.
    /// </summary>
    public class FuelTypeRepo : IFuelTypeRepo
    {
        private readonly FleetContext _db;

        public FuelTypeRepo(FleetContext db) 
        {
            _db = db;
        }
        #region INDEX-GET ALL
        /// <summary>
        /// Gets all fuelType that are active from DB.
        /// </summary>
        /// <returns></returns>
        public List<FuelType> GetAll()
        {
            return _db.FuelType
                .Where(x => x.IsActive)
                .ToList();
        }
        #endregion

        #region DETAILS
        /// Gets data of specific fuelType from DB.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public FuelType Get(int id)
        {
            return _db.FuelType
                .FirstOrDefault(x => x.IsActive);
        }
        #endregion

        #region REMOVE
        /// <summary>
        /// Handles soft deleting of a specific fuelType.
        /// </summary>
        /// <param name="id">id of target fuelType</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Delete(int id)
        {
            var FuelType = _db.FuelType.First(x => x.FuelTypeID == id);
            FuelType.IsActive = false;
            _db.SaveChanges();

        }
        #endregion

        #region CREATE/ADD
        /// <summary>
        /// Adds a new fuelType to db.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Add(FuelType entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region EDIT/UPDATE
        public int Update(FuelType entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region HELPERS
        /// <summary>
        /// Function for deleting fuelCardfueltype from fuelCard (linking table)
        /// </summary>
        /// <param name="fuelCardId">id of fuelCard</param>
        /// <param name="fuelTypeId">id of fuelType</param>
        public void DeleteFuelCardFuelType(int fuelTypeId, int fuelCardId)
        {
            var fuelType = _db.FuelCardFuelType
                .FirstOrDefault(x =>
                x.FuelTypeID == fuelTypeId &&
                x.FuelCardID == fuelCardId);

            if (fuelType != null)
            {
                _db.FuelCardFuelType.Remove(fuelType);
            }
        }
        #endregion
    }
}
