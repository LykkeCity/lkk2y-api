using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Lykke.Service.Lkk2Y_Api.Models
{
    public class SubscribeModel
    {
        [JsonProperty("email")]        
        public string Email { get; set; }
    }
    
}
