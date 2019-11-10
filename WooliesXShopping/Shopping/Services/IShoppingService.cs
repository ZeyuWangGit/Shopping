﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopping.Models;
using Shopping.Models.Trolley;

namespace Shopping.Services
{
    public interface IShoppingService
    {
        UserInfo GetUserInfo();
        Task<IEnumerable<Product>> SortProducts(string sortOption);
        Task<decimal> CalculateTrolleyTotal(Trolley trolley);
        decimal CalculateTrolley(Trolley trolley);
    }
}