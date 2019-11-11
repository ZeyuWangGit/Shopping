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
    public class ShoppingService: IShoppingService
    {
        private readonly Resource _resource;
        private readonly IShoppingResourceService _shoppingResourceService;

        public ShoppingService(IOptions<Resource> resourceOptions,
            IShoppingResourceService shoppingResourceService)
        {
            _resource = resourceOptions.Value;
            _shoppingResourceService = shoppingResourceService;
        }

        public UserInfo GetUserInfo()
        {
            return new UserInfo
            {
                Name = _resource.Name,
                Token = _resource.Token
            };
        }

        public async Task<IEnumerable<Product>> SortProducts(ProductSortOption sortOption)
        {
            var products = await _shoppingResourceService.GetProducts();

            switch (sortOption)
            {
                case ProductSortOption.Low:
                    return SortProductsByLowPrice(products.ToList());
                case ProductSortOption.High:
                    return SortProductsByHighPrice(products.ToList());
                case ProductSortOption.Ascending:
                    return SortProductsByAscendingName(products.ToList());
                case ProductSortOption.Descending:
                    return SortProductsByDescendingName(products.ToList());
                case ProductSortOption.Recommended:
                    return await SortProductsByRecommended(products.ToList());
                default:
                    return products.ToList();
            }
        }

        public async Task<decimal> CalculateTrolleyTotal(Trolley trolley)
        {
            return await _shoppingResourceService.CalculateTrolleyViaApi(trolley);
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

        private async Task<Dictionary<string, int>> GetProductPopularity(List<Product> products)
        {
            var shoppingHistory = await _shoppingResourceService.GetShoppingHistory();
            var productsPopularity = new Dictionary<string, int>();
            foreach (var customerShopperHistory in shoppingHistory)
            {
                var customerProducts = customerShopperHistory.Products;
                foreach (var product in customerProducts)
                {
                    if (productsPopularity.ContainsKey(product.Name))
                    {
                        productsPopularity[product.Name] = productsPopularity[product.Name] + 1;
                    }
                    else
                    {
                        productsPopularity[product.Name] = 1;
                    }
                }
            }

            foreach (var product in products)
            {
                if (!productsPopularity.ContainsKey(product.Name))
                {
                    productsPopularity[product.Name] = 0;
                }
            }

            return productsPopularity;
        }

        private static IEnumerable<Product> SortProductsByLowPrice(IEnumerable<Product> products)
        {
            return products.OrderBy(o => o.Price).ToList();
        }

        private static IEnumerable<Product> SortProductsByHighPrice(IEnumerable<Product> products)
        {
            return products.OrderByDescending(o => o.Price).ToList();
        }

        private static IEnumerable<Product> SortProductsByAscendingName(IEnumerable<Product> products)
        {
            return products.OrderBy(o => o.Name, StringComparer.OrdinalIgnoreCase).ToList();
        }

        private static IEnumerable<Product> SortProductsByDescendingName(IEnumerable<Product> products)
        {
            return products.OrderByDescending(o => o.Name, StringComparer.OrdinalIgnoreCase).ToList();
        }

        private async Task<IEnumerable<Product>> SortProductsByRecommended(IEnumerable<Product> products)
        {
            var productLists = products.ToList();
            var productPopularity = await GetProductPopularity(productLists);

            productLists.Sort((a, b) =>
            {
                var aPop = productPopularity[a.Name];
                var bPop = productPopularity[b.Name];
                if (aPop > bPop)
                {
                    return -1;
                }

                if(aPop == bPop)
                {
                    return 0;
                }

                return 1;
            });

            return productLists;
        }
    }

}
