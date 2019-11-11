using Shopping.Models;
using Shopping.Models.Trolley;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Services
{
    public class TrolleyService: ITrolleyService
    {
        private readonly IResourceHttpClientService _resourceHttpClientService;

        public TrolleyService(IResourceHttpClientService resourceHttpClientService)
        {
            _resourceHttpClientService = resourceHttpClientService;
        }

        public async Task<decimal> CalculateTrolleyTotalViaApi(Trolley trolley)
        {
            return await _resourceHttpClientService.CalculateTrolleyViaApi(trolley);
        }

        public decimal CalculateTrolley(Trolley trolley)
        {
            var prices = new List<decimal> {ApplySpecial(trolley.Specials, GetProductsList(trolley))};
            for (var i = 1; i < trolley.Specials.Count; i++)
            {
                var rotateList = trolley.Specials.Skip(1).Concat(trolley.Specials.Take(1));
                prices.Add(ApplySpecial(rotateList, GetProductsList(trolley)));
            }
            prices.Sort();
            return prices.FirstOrDefault();
        }

        private Dictionary<string, Product> GetProductsList(Trolley trolley)
        {
            var products = new Dictionary<string, Product>();
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

            return products;
        }

        private decimal ApplySpecial(IEnumerable<Special> specials, Dictionary<string, Product> products)
        {
            decimal totalPrice = 0;
            foreach (var special in specials)
            {
                var satisfiedTime = int.MaxValue;
                foreach (var items in special.Quantities)
                {
                    if (products.TryGetValue(items.Name, out var specialProd) && items.Quantity > 0)
                    {
                        var time = (int)(specialProd.Quantity / items.Quantity);
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

            foreach (var (_, value) in products)
            {
                totalPrice += value.Price * value.Quantity;
            }

            return totalPrice;
        }

    }

}
