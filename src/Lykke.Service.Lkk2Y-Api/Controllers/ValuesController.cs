using Microsoft.AspNetCore.Mvc;
using Lykke.Service.Lkk2Y_Api.Models;
using Lykke.Service.Lkk2Y_Api.Core;
using System;
using System.Threading.Tasks;
using Lykke.Service.Lkk2Y_Api.Services;
using Common;

namespace Lykke.Service.Lkk2Y_Api.Controllers
{
    public class ValuesController : Controller
    {
        private readonly ILkk2YOrdersRepository _lkk2YOrdersRepository;
        private readonly ILkk2yInfoRepository _lkk2YInfoRepository;

        private readonly RateConverterService _rateConverterSrv;

        public ValuesController(ILkk2YOrdersRepository lkk2YOrdersRepository, 
        ILkk2yInfoRepository lkk2YInfoRepository, RateConverterService rateConverterSrv)
        {
            _lkk2YOrdersRepository = lkk2YOrdersRepository;
            _lkk2YInfoRepository = lkk2YInfoRepository;
            _rateConverterSrv = rateConverterSrv;
        }

        [HttpPost("api/subscribe")]
        public object Subscribe([FromBody]SubscribeModel model)
        {
            return new { result = "OK", model }; 
        }

        [HttpPost("api/order")]
        public async Task<object> Order([FromBody] OrderModel model)
        {


            if (model == null){
                Console.WriteLine("!!!! Model is null !!!!!");
                return NotFound();
            }

            Console.WriteLine("Order:" + model.ToJson());

            model.Currency = model.Currency.Trim();

            model.UsdAmount = model.Currency == "USD"
                ? model.Amount
                : model.UsdAmount = await _rateConverterSrv.ConvertAsync(model.Currency, "USD", model.Amount);


            model.Ip = this.GetIp();

            await _lkk2YOrdersRepository.RegisterAsync(DateTime.UtcNow, model);

            return new
            {
                result = "OK",
                model
            };

        }

        [HttpPost("api/convert")]
        public async Task<object> Convert([FromBody]ConvertModel model)
        {


            if (model == null){
                Console.WriteLine("Convert: !!!! Model is null !!!!!");
                return NotFound();
            }

            Console.WriteLine("Convert:" + model.ToJson());

            model.From = model.From.Trim();
            model.To = model.To.Trim();

            var amount = await _rateConverterSrv.ConvertAsync(model.From, model.To, model.Amount);
            return new { asset = model.To, amount };
        }

        [HttpGet("api/info")]
        public async Task<object> Info()
        {
            var infoTask = _lkk2YInfoRepository.GetInfoAsync();
            var totalTaks = _lkk2YOrdersRepository.GetUsdTotalAsync();

            await infoTask;
            await totalTaks;

            return new
            {
                startDate = 1513080000,
                fundsRecieved = infoTask.Result.FundsRecieved,
                fundsGoal = infoTask.Result.FundsGoal,
                fundsTotal = totalTaks.Result
            };
        }

    }

}
