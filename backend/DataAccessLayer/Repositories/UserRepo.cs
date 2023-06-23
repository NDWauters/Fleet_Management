using DataAccessLayer.Interfaces;
using DataAccessLayer.Model;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly FleetContext _db;

        public UserRepo(FleetContext db)
        {
            _db = db;
        }

        public int Add(User entity)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public User Get(int id)
        {
            return _db.User
                .FirstOrDefault(x => x.UserID == id);
        }

        public User Get(string userName)
        {
            return _db.User
                .FirstOrDefault(x => x.UserName == userName);
        }

        public List<User> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public int Update(User entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
