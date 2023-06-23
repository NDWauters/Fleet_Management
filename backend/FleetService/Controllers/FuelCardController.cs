using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.ViewModels.Authorization;
using BusinessLogicLayer.ViewModels.FuelCard;
using Microsoft.AspNetCore.Mvc;

namespace FleetService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class FuelCardController : Microsoft.AspNetCore.Mvc.Controller
    {
        #region PROPS
        private readonly IFuelCardService _fuelCardService;
        private readonly ILogger _logger;
        #endregion

        #region CONSTR
        public FuelCardController(
            IFuelCardService fuelCardService, ILoggerFactory loggerFactor)
        {
            _fuelCardService = fuelCardService;
            _logger = loggerFactor.AddFile("ControllerLog/FuelCardControllerLogs.txt").CreateLogger("FuelCard");
        }
        #endregion

        #region INDEX
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var models = _fuelCardService.TryGet();
                return Ok(models);
            }
            catch (Exception e)
            {
                _logger.LogError($"- INDEX - {e.Message}");

                return NotFound(e.Message);
            }
        }
        #endregion

        #region DETAILS
        /// <summary>
        /// Details fuel card with ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            try
            {
                var model = _fuelCardService.TryGet(id);

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
                _fuelCardService.TryRemove(id);

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
                var model = _fuelCardService.TryInit();

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
        public IActionResult Create([FromBody]FuelCardModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var id = _fuelCardService.TryCreate(model);

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
                var model = _fuelCardService.TryEdit(id);

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
        public IActionResult Edit([FromBody] FuelCardModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var id = _fuelCardService.TryEdit(model);

                return CreatedAtAction(nameof(Edit), new { id });
            }
            catch (Exception e)
            {
                _logger.LogError($"- EDIT PUT - {e.Message}");

                return BadRequest(e.Message);
            }
        }
        #endregion
        #endregion
    }
}
