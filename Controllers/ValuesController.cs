using System;
using lkk2yapi.Models;
using Microsoft.AspNetCore.Mvc;

namespace lkk2yapi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {

        [HttpGet]
        public object IsAlive()
        {
            return new
            {
                Version = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application
                    .ApplicationVersion,
                Env = Environment.GetEnvironmentVariable("Env")
            };
        }


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
