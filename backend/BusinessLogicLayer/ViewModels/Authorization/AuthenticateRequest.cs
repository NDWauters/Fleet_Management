using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.ViewModels.Authorization
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
