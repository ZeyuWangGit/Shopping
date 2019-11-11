using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shopping.Common;
using Shopping.Models;
using Shopping.Models.Trolley;
using Shopping.Services;

namespace Shopping.Controllers
{
    [Route("api")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        private readonly IShoppingService _shoppingService;
        private readonly ILogger<ShoppingController> _logger;
        public ShoppingController(ILogger<ShoppingController> logger, 
            IShoppingService shoppingService)
        {
            _shoppingService = shoppingService;
            _logger = logger;
        }

        // GET api/user
        [HttpGet("user")]
        public ActionResult<UserInfo> GetUserInfo()
        {
            _logger.LogInformation("api/user has been called...");

            var response = _shoppingService.GetUserInfo();

            _logger.LogInformation($"api/user response {response.Name} {response.Token}");

            return response;
        }

        // GET api/sort
        [HttpGet("sort")]
        public async Task<ActionResult<IEnumerable<Product>>> GetSortedProducts(string sortOption)
        {
            _logger.LogTrace($"api/sort catch with sortOption {sortOption}");

            if (string.IsNullOrWhiteSpace(sortOption) || !ModelState.IsValid)
            {
                return BadRequest("Required Query Information Not Provided");
            }

            Enum.TryParse(sortOption, out ProductSortOption productSortOption);

            var sortedProducts = await _shoppingService.SortProducts(productSortOption);

            _logger.LogTrace($"api/sort products sorted executed with sortOption {sortOption}");

            return Ok(sortedProducts);
        }

        // POST api/trolleyTotal
        [HttpPost("trolleyTotal")]
        public async Task<ActionResult<decimal>> GetTrolleyTotal([FromBody] Trolley trolley)
        {
            _logger.LogTrace($"api/trolleyTotal has been called...");

            if (!ModelState.IsValid)
            {
                return BadRequest("Not all data is valid for trolleyCalculator");
            }

            var result = await _shoppingService.CalculateTrolleyTotal(trolley);

            _logger.LogTrace($"api/trolleyTotal has been executed");

            return Ok(result);
        }

    }
}
