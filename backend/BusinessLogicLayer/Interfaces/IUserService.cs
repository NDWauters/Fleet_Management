using BusinessLogicLayer.ViewModels.Authorization;
using DataAccessLayer.Model;

namespace BusinessLogicLayer.Interfaces
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        User Get(int id);
    }
}
