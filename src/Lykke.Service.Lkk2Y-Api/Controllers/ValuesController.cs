using Microsoft.AspNetCore.Mvc;
using Lykke.Service.Lkk2Y_Api.Models;
using System.Text;

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

        [HttpGet("api/info")]
        public object Info()
        {
            return new {startDate = 1513987200000, fundsRecieved = 12000, fundsGoal = 100000};
        }


        [HttpGet("api/test")]
        public string Test()
        {
            var sr = new StringBuilder();

            foreach (var header in Request.Headers)
                sr.Append(header.Key+":"+header.Value+";   ");


            return sr.ToString();
        }

    }

}
