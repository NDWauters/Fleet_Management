using DataAccessLayer.Interfaces;
using DataAccessLayer.Model;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    /// <summary>
    /// Class responsible for taking care of communication with db.
    /// </summary>
    public class BrandRepo : IBrandRepo
    {
        private readonly FleetContext _db;

        public BrandRepo(FleetContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Adds a new brand to db.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int Add(Brand entity)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Handles soft deleting of a specific brand.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets data of specific brand from DB.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Brand Get(int id)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets all brand that are active from DB.
        /// </summary>
        /// <returns></returns>
        public List<Brand> GetAll()
        {
            return _db.Brand
                .Where(x => x.IsActive)
                .ToList();
        }

        public int Update(Brand entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
