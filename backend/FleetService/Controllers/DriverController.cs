using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels.Authorization;
using BusinessLogicLayer.ViewModels.Driver;
using Microsoft.AspNetCore.Mvc;

namespace FleetService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class DriverController : ControllerBase
    {
        #region PROPS
        private readonly IDriverService _driverService;
        private readonly ILogger _logger;
        #endregion

        #region CONSTR
        public DriverController(
            IDriverService driverService,
            ILoggerFactory loggerFactor)
        {
            _driverService = driverService;
            _logger = loggerFactor.AddFile("ControllerLog/DriverControllerLogs.txt").CreateLogger("Driver");

        }
        #endregion

        #region INDEX-GET ALL
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var models = _driverService.TryGet();

                return Ok(models);
            }
            catch (Exception e)
            {
                _logger.LogError($"- INDEX GET ALL - {e.Message}");

                return NoContent();
            }
        }
        #endregion

        #region DETAILS
        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            try
            {
                var model = _driverService.TryGet(id);

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError($"- DETAILS - {e.Message}");

                return NotFound(e.Message);
            }
        }
        #endregion

        #region REMOVE
        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            try
            {
                _driverService.TryRemove(id);

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"- REMOVE - {e.Message}");

                return BadRequest(e.Message);
            }
        }
        #endregion

        #region CREATE/ADD

        #region CREATE/ADD - (GET)
        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                var model = _driverService.TryInit();

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError($"- CREATE GET - {e.Message}");

                return BadRequest(e.Message);
            }
        }
        #endregion

        #region CREATE/ADD - (POST)
        [HttpPost]
        public IActionResult Create([FromBody] DriverModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var id = _driverService.TryCreate(model);

                return CreatedAtAction(nameof(Create), new { id });
            }
            catch (Exception e)
            {
                _logger.LogError($"- CREATE POST - {e.Message}");

                return BadRequest(e.Message);
            }
        }
        #endregion

        #endregion

        #region EDIT/UPDATE

        #region EDIT/UPDATE - (GET)
        [HttpGet("{id}")]
        public IActionResult Edit(int id)
        {
            try
            {
                var model = _driverService.TryEdit(id);

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError($"- EDIT GET - {e.Message}");

                return BadRequest(e.Message);
            }
        }
        #endregion

        #region EDIT/UPDATE - (POST)
        [HttpPut]
        public IActionResult Edit([FromBody] DriverModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var id = _driverService.TryEdit(model);

                return CreatedAtAction(nameof(Edit), new { id });
            }
            catch (Exception e)
            {
                _logger.LogError($"- EDIT POST - {e.Message}");

                return BadRequest(e.Message);
            }
        }
        #endregion

        #endregion

    }
}
