using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FleetManagement.Entities
{
    public class Vehicle
    {
        [JsonProperty(PropertyName = "vin")]
        public string VehicleId { get; set; }
        [JsonProperty(PropertyName = "regid")]
        public string RegistrationNumber { get; set; }
    }
}
