using Newtonsoft.Json;

namespace Lykke.Service.Lkk2Y_Api.Models
{
    public class ConvertModel
    {

        [JsonProperty("from")] 
        public string From { get; set; }
        
        [JsonProperty("amount")] 
        public double Amount { get; set; }

        [JsonProperty("to")] 
        public string To { get; set; }
    }

}