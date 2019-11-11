using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shopping.Models;
using Shopping.Models.Trolley;

namespace Shopping.Services
{
    public class ResourceHttpClientService : IResourceHttpClientService
    {
        private readonly Resource _resource;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<ResourceHttpClientService> _logger;

        public const string TrolleyCalculatorResourceApi = "{0}/trolleyCalculator?token={1}";
        public const string ProductListApi = "{0}/products?token={1}";
        public const string ShoppingHistoryApi = "{0}/shopperHistory?token={1}";

        public ResourceHttpClientService(IOptions<Resource> resourceOptions, IHttpClientFactory clientFactory, ILogger<ResourceHttpClientService> logger)
        {
            _resource = resourceOptions.Value;
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<decimal> CalculateTrolleyViaApi(Trolley trolley)
        {
            var trolleyCalculatorUrl = string.Format(TrolleyCalculatorResourceApi, _resource.ResourceUrl, _resource.Token);

            return await ResourceHttpClient<decimal>(HttpMethod.Post, trolleyCalculatorUrl, JsonConvert.SerializeObject(trolley));
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var productUrl = string.Format(ProductListApi, _resource.ResourceUrl, _resource.Token);

            return await ResourceHttpClient<IEnumerable<Product>>(HttpMethod.Get, productUrl);
        }

        public async Task<IEnumerable<ShopperHistory>> GetShoppingHistory()
        {
            var shoppingHistoryUrl = string.Format(ShoppingHistoryApi, _resource.ResourceUrl, _resource.Token);

            return await ResourceHttpClient<IEnumerable<ShopperHistory>>(HttpMethod.Get, shoppingHistoryUrl);
        }

        private async Task<T> ResourceHttpClient<T>(HttpMethod httpMethod, string url, string jsonContent = "")
        {
            try
            {
                using (var request = new HttpRequestMessage(httpMethod, url))
                {
                    using (var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json"))
                    {
                        request.Content = stringContent;

                        using (var client = _clientFactory.CreateClient())
                        {
                            using (var response = await client.SendAsync(request))
                            {
                                response.EnsureSuccessStatusCode();
                                var result = await response.Content.ReadAsStringAsync();
                                return JsonConvert.DeserializeObject<T>(result);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in ResourceHttpClient with url {url} and jsonContent {jsonContent}");
                throw;
            }
        }
    }
}
