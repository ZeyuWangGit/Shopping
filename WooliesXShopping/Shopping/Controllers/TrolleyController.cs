using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shopping.Models.Trolley;
using Shopping.Services;
using System.Threading.Tasks;

namespace Shopping.Controllers
{
    [Route("api")]
    [ApiController]
    public class TrolleyController : ControllerBase
    {
        private readonly ITrolleyService _trolleyService;
        private readonly ILogger<TrolleyController> _logger;
        public TrolleyController(ILogger<TrolleyController> logger, 
            ITrolleyService trolleyService)
        {
            _trolleyService = trolleyService;
            _logger = logger;
        }

        // POST api/trolleyTotal
        [HttpPost("trolleyTotal")]
        public async Task<ActionResult<decimal>> GetTrolleyTotal([FromBody] Trolley trolley)
        {
            _logger.LogTrace($"api/trolleyTotal has been called...");

            if (!ModelState.IsValid)
            {
                return BadRequest("Trolley Data Is Not Valid");
            }

            var result = await _trolleyService.CalculateTrolleyTotal(trolley);

            _logger.LogTrace($"api/trolleyTotal has been executed");

            return Ok(result);
        }

    }
}
