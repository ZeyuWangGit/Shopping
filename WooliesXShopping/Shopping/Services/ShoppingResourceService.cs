using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shopping.Models;
using Shopping.Models.Trolley;

namespace Shopping.Services
{
    public class ShoppingResourceService : IShoppingResourceService
    {
        private readonly Resource _resource;
        private readonly IHttpClientFactory _clientFactory;

        public ShoppingResourceService(IOptions<Resource> resourceOptions, IHttpClientFactory clientFactory)
        {
            _resource = resourceOptions.Value;
            _clientFactory = clientFactory;
        }

        public async Task<decimal> CalculateTrolleyViaApi(Trolley trolley)
        {
            var trolleyCalculatorUrl = $"{_resource.ResourceUrl}/trolleyCalculator?token={_resource.Token}";

            var request = new HttpRequestMessage(HttpMethod.Post, trolleyCalculatorUrl)
            {
                Content = new StringContent(JsonConvert.SerializeObject(trolley), Encoding.UTF8, "application/json")
            };


            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<decimal>(result);
            }

            return 0;
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            var productUrl = $"{_resource.ResourceUrl}/products?token={_resource.Token}";

            var request = new HttpRequestMessage(HttpMethod.Get, productUrl);

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Product>>(result);
            }

            return Array.Empty<Product>();
        }
        public async Task<IEnumerable<ShopperHistory>> GetShoppingHistory()
        {
            var shoppingHistoryUrl = $"{_resource.ResourceUrl}/shopperHistory?token={_resource.Token}";

            var request = new HttpRequestMessage(HttpMethod.Get, shoppingHistoryUrl);

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<ShopperHistory>>(result);
            }

            return Array.Empty<ShopperHistory>();
        }
    }
}
