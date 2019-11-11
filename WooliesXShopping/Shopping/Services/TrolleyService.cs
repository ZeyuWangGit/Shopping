using System;
using Microsoft.Extensions.Options;
using Shopping.Models;
using Shopping.Models.Trolley;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopping.Common;

namespace Shopping.Services
{
    public class TrolleyService: ITrolleyService
    {
        private readonly IResourceHttpClientService _resourceHttpClientService;

        public TrolleyService(IResourceHttpClientService resourceHttpClientService)
        {
            _resourceHttpClientService = resourceHttpClientService;
        }

        public async Task<decimal> CalculateTrolleyTotal(Trolley trolley)
        {
            return await _resourceHttpClientService.CalculateTrolleyViaApi(trolley);
        }

        public decimal CalculateTrolley(Trolley trolley)
        {
            var products = new Dictionary<string, Product>();
            decimal totalPrice = 0;
            foreach (var product in trolley.Products)
            {
                products.Add(product.Name, new Product
                {
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 0
                });
            }

            foreach (var quantity in trolley.Quantities)
            {
                if (products.ContainsKey(quantity.Name))
                {
                    products[quantity.Name].Quantity = quantity.Quantity;
                }
            }

            foreach (var special in trolley.Specials)
            {
                var satisfiedTime = int.MaxValue;
                foreach (var items in special.Quantities)
                {
                    if (products.TryGetValue(items.Name, out var specialProd))
                    {
                        var time = (int) (specialProd.Quantity / items.Quantity);
                        if (time <= satisfiedTime)
                        {
                            satisfiedTime = time;
                        }
                    }
                }

                foreach (var items in special.Quantities)
                {
                    if (products.TryGetValue(items.Name, out var specialProd))
                    {
                        specialProd.Quantity -= satisfiedTime * items.Quantity;
                    }
                }

                if (satisfiedTime == int.MaxValue)
                {
                    satisfiedTime = 0;
                }
                totalPrice += satisfiedTime * special.Total;
            }

            foreach (KeyValuePair<string, Product> entry in products)
            {
                totalPrice += entry.Value.Price * entry.Value.Quantity;
            }

            return totalPrice;
        }
    }

}
