using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public UserController(
            IUserService userService, ILoggerFactory loggerFactor)
        {
            _userService = userService;
            _logger = loggerFactor.AddFile("ControllerLog/UserControllerLogs.txt").CreateLogger("User");
        }

        [BusinessLogicLayer.ViewModels.Authorization.AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate([FromBody] AuthenticateRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger?.LogError($"- Authenticate - {ModelState}");
                    return BadRequest(ModelState);
                }

                _logger?.LogInformation($"- UserLogin - {ModelState}");
                var response = _userService.Authenticate(model);

                // return basic user info and authentication token
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
