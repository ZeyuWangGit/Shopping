using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopping.Common;
using Shopping.Models;

namespace Shopping.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> SortProducts(ProductSortOption sortOption);
    }
}
