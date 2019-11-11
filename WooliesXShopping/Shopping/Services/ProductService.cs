using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopping.Common;
using Shopping.Models;

namespace Shopping.Services
{
    public class ProductService: IProductService
    {
        private readonly IResourceHttpClientService _resourceHttpClientService;

        public ProductService(IResourceHttpClientService resourceHttpClientService)
        {
            _resourceHttpClientService = resourceHttpClientService;
        }

        public async Task<IEnumerable<Product>> SortProducts(ProductSortOption sortOption)
        {
            var products = await _resourceHttpClientService.GetProducts();

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

        private async Task<Dictionary<string, int>> GetProductPopularity(List<Product> products)
        {
            var shoppingHistory = await _resourceHttpClientService.GetShoppingHistory();
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

                if (aPop == bPop)
                {
                    return 0;
                }

                return 1;
            });

            return productLists;
        }
    }
}
