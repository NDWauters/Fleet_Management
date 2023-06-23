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

    public class AddressRepo : IAddressRepo
    {
        private readonly FleetContext _db;

        public AddressRepo(FleetContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Adds a new address to db.
        /// </summary>
        /// <param name="entity">Address object</param>
        /// <returns>id of new address</returns>
        public int Add(Address entity)
        {
            _db.Address.Add(entity);
            _db.SaveChanges();
            //
            return entity.AddressID;

        }

        /// <summary>
        /// Handles soft deleting of a specific address
        /// </summary>
        /// <param name="id">id of target address</param>
        public void Delete(int id)
        {
            var address = _db.Address.First(x => x.AddressID == id);
            //
            address.IsActive = false;
            //
            _db.SaveChanges();
        }

        /// <summary>
        /// Gets data of specific address from DB.
        /// </summary>
        /// <param name="id">id of specific address</param>
        /// <returns>Address object</returns>
        public Address Get(int id)
        {
            return _db.Address
                .Include("Driver")
                .FirstOrDefault(x => x.AddressID == id);

        }

        /// <summary>
        /// Gets all addresses that are active from DB
        /// </summary>
        /// <returns>List of Address objects</returns>
        public List<Address> GetAll()
        {
            return _db.Address
                .Include("Driver")
                .Where(x => x.IsActive)
                .ToList();
        }

        /// <summary>
        /// Update an address.
        /// </summary>
        /// <param name="entity">Address object</param>
        /// <returns>id of address</returns>
        public int Update(Address entity)
        {
            _db.SaveChanges();

            return entity.AddressID;
        }
    }
}
