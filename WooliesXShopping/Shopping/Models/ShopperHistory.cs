using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Shopping.Models
{
    public class ShopperHistory
    {
        [JsonProperty("customerId")]
        public int CustomerId { get; set; }
        [JsonProperty("products")]
        public List<Product> Products { get; set; }
    }
}
