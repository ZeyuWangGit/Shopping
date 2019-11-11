using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Shopping.Models.Trolley
{
    public class ProductQuantity
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
