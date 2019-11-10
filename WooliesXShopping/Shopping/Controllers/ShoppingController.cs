﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public ShoppingController(IShoppingService shoppingService)
        {
            _shoppingService = shoppingService;
        }

        // GET api/user
        [HttpPost("user")]
        public UserInfo GetUserInfo()
        {
            return _shoppingService.GetUserInfo();
        }

        // GET api/values/5
        [HttpPost("sort")]
        public async Task<IEnumerable<Product>> GetSortedProducts(string sortOption)
        {
            return await _shoppingService.SortProducts(sortOption);
        }

        // POST api/values
        [HttpPost("trolleyTotal")]
        public async Task<decimal> GetTrolleyTotal([FromBody] Trolley trolley)
        {
            return await _shoppingService.CalculateTrolleyTotal(trolley);
        }

    }
}