using Lykke.Service.Lkk2Y_Api.Core;
using Newtonsoft.Json;

namespace Lykke.Service.Lkk2Y_Api.Models
{
    public class OrderModel :ILkk2YOrder
    {
        [JsonProperty("amount")]
        public double Amount {get;set;}

        [JsonProperty("country")]        
        public string Country {get;set;}

        [JsonProperty("currency")]         
        public string Currency {get;set;}

        [JsonProperty("email")]         
        public string Email { get; set; }

        [JsonProperty("first_name")]          
        public string FirstName { get; set; }

        [JsonProperty("last_name")] 
        public string LastName { get; set; }

        public double UsdAmount { get; set; }
        public string Ip { get; set; }
    }
}
