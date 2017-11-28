using Microsoft.AspNetCore.Mvc;
using Lykke.Service.Lkk2Y_Api.Models;

namespace Lykke.Service.Lkk2Y_Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpPost]
        public object Subscribe([FromBody]SubscribeModel model)
        {
            return new {result = "OK"};
        }

        // PUT api/values/5
        [HttpPost]
        public object Order([FromBody]string model)
        {
            return new {result = "OK"};
        }
    }
}
