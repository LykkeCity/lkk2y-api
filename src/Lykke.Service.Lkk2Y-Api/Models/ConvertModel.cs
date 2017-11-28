using Newtonsoft.Json;

namespace Lykke.Service.Lkk2Y_Api.Models
{
    public class ConvertModel
    {
        public class AssetFromModel{
            public string AssetId { get; set; }
            public double Amount { get; set; }
        }

        [JsonProperty("baseAssetId")] 
        public string BaseAssetId { get; set; }
        
        [JsonProperty("orderAction")] 
        public string OrderAction { get; set; }

        [JsonProperty("assetsFrom")] 
        public AssetFromModel AssetFrom { get; set; }
    }
    
}