using DataAccessLayer.Interfaces;
using DataAccessLayer.Model;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class VehicleTypeRepo : IVehicleTypeRepo
    {
        /// <summary>
        /// Class responsible for taking care of communication with db.
        /// </summary>
        private readonly FleetContext _db;

        public VehicleTypeRepo(FleetContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Adds a new vehicleType to db.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int Add(VehicleType entity)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Handles soft deleting of a specific vehicleType.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets data of specific vehicleType from DB.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public VehicleType Get(int id)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets all vehicleType that are active from DB.
        /// </summary>
        /// <returns></returns>
        public List<VehicleType> GetAll()
        {
            return _db.VehicleType
                .Where(x => x.IsActive)
                .ToList();
        }

        public int Update(VehicleType entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
