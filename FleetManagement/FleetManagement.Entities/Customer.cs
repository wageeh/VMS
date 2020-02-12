using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FleetManagement.Entities
{
    public class Customer
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "vehicles")]
        public Vehicle[] Vehicles { get; set; }

        [JsonProperty(PropertyName = "address")]
        public Address Address { get; set; }

        public Customer()
        {
            Id = Guid.NewGuid().ToString();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}
