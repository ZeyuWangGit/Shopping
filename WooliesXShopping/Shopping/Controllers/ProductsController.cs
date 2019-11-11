using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shopping.Common;
using Shopping.Models;
using Shopping.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shopping.Controllers
{
    [Route("api")]
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;

        public ProductsController(ILogger<ProductsController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet("sort")]
        public async Task<ActionResult<IEnumerable<Product>>> GetSortedProducts(string sortOption)
        {
            _logger.LogTrace($"api/sort has been called with sortOption {sortOption}");

            if (string.IsNullOrWhiteSpace(sortOption) || !ModelState.IsValid)
            {
                return BadRequest("Required Query SortOptions Is Not Provided Or Valid");
            }

            Enum.TryParse(sortOption, out ProductSortOption productSortOption);

            var sortedProducts = await _productService.SortProducts(productSortOption);

            _logger.LogTrace($"api/sort products sorted executed with sortOption {sortOption}");

            return Ok(sortedProducts);
        }
    }
}
