using DataAccessLayer.Model;

namespace DataAccessLayer.Interfaces
{
    public interface IUserRepo : IRepository<User>
    {
        User Get(string token);
    }
}
