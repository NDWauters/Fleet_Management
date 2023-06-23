using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels.Authorization;
using BusinessLogicLayer.ViewModels.Vehicle;
using Microsoft.AspNetCore.Mvc;

namespace FleetService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class VehicleController : ControllerBase
    {
        #region PROPS
        private readonly IVehicleService _vehicleService;
        private readonly ILogger _logger;
        #endregion

        #region CONSTR
        public VehicleController(
            IVehicleService vehicleService,
            ILoggerFactory loggerFactor)
        {
            _vehicleService = vehicleService;
            _logger = loggerFactor.AddFile("ControllerLog/VehicleControllerLogs.txt").CreateLogger("Vehicle");
        }
        #endregion

        #region INDEX
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var models = _vehicleService.TryGet();

                return Ok(models);
            }
            catch (Exception e)
            {
                _logger.LogError($"- INDEX - {e.Message}");

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
                var model = _vehicleService.TryGet(id);

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
                _vehicleService.TryRemove(id);

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"- REMOVE - {e.Message}");

                return BadRequest(e.Message);
            }
        }
        #endregion

        #region CREATE
        #region CREATE - GET
        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                var model = _vehicleService.TryInit();

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError($"- CREATE GET - {e.Message}");

                return BadRequest(e.Message);
            }
        }
        #endregion

        #region CREATE - POST
        [HttpPost]
        public IActionResult Create([FromBody]VehicleModel model)
        {
            try
            {
                if (!ModelState.IsValid) 
                {
                    return BadRequest(ModelState);
                }

                var id = _vehicleService.TryCreate(model);

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

        #region EDIT
        #region EDIT - GET
        [HttpGet("{id}")]
        public IActionResult Edit(int id) 
        {
            try
            {
                var model = _vehicleService.TryEdit(id);

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError($"- EDIT GET - {e.Message}");

                return BadRequest(e.Message);
            }
        }
        #endregion

        #region EDIT - PUT
        [HttpPut]
        public IActionResult Edit([FromBody]VehicleModel model) 
        {
            try
            {
                if (!ModelState.IsValid) 
                { 
                    return BadRequest(ModelState); 
                }

                var id = _vehicleService.TryEdit(model);

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

