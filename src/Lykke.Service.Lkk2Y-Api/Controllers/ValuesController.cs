using Microsoft.AspNetCore.Mvc;
using Lykke.Service.Lkk2Y_Api.Models;

namespace Lykke.Service.Lkk2Y_Api.Controllers
{
    public class ValuesController : Controller
    {
        [HttpPost("api/subscribe")]
        public object Subscribe([FromBody]SubscribeModel model)
        {
            return new {result = "OK", model};
        }

        [HttpPost("api/order")]
        public object Order([FromBody]OrderModel model)
        {
            return new {result = "OK", model};
        }

        [HttpPost("api/convert")]
        public object Convert([FromBody]ConvertModel model)
        {
            return new {asset = model.To, amount = model.Amount*2};
        }

    }

}
