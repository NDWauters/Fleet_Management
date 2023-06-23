using BusinessLogicLayer.Authorization;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels.Authorization;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Model;

namespace BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private IJwtUtils _jwtUtils;

        public UserService(
            IUserRepo userRepo,
            IJwtUtils jwtUtils)
        {
            _userRepo = userRepo;
            _jwtUtils = jwtUtils;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _userRepo.Get(model.Username);

            // validate
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                throw new UserException("Username or password is incorrect");

            // authentication successful
            var response = new AuthenticateResponse 
            { 
                FirstName = user.FirstName,
                LastName = user.LastName,
                ID = user.UserID,
                Username = user.UserName
            };

            response.Token = _jwtUtils.GenerateToken(user);

            return response;
        }

        public User Get(int id)
        {
            return GetUser(id);
        }

        private User GetUser(int id)
        {
            var user = _userRepo.Get(id);

            if (user == null) 
            {
                throw new KeyNotFoundException("User not found");
            }
            
            return user;
        }
    }
}
