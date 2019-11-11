using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopping.Models;
using Shopping.Models.Trolley;

namespace Shopping.Services
{
    public interface IResourceHttpClientService
    {
        Task<decimal> CalculateTrolleyViaApi(Trolley trolley);
        Task<IEnumerable<Product>> GetProducts();
        Task<IEnumerable<ShopperHistory>> GetShoppingHistory();
    }
}
