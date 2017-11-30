using Microsoft.AspNetCore.Mvc;
using Lykke.Service.Lkk2Y_Api.Models;
using System.Text;
using Lykke.Service.Lkk2Y_Api.Core;
using System;
using System.Threading.Tasks;
using Lykke.Service.Lkk2Y_Api.Services;

namespace Lykke.Service.Lkk2Y_Api.Controllers
{
    public class ValuesController : Controller
    {
        private readonly ILkk2yOrdersRepository _lkk2YOrdersRepository;
        private readonly ILkk2yInfoRepository _lkk2YInfoRepository;

        private readonly RateConverterService _rateConverterSrv;

        public ValuesController(ILkk2yOrdersRepository lkk2YOrdersRepository, 
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
        public async Task<object> Order([FromBody]OrderModel model)
        {

            try
            {
                await _lkk2YOrdersRepository.RegisterAsync(DateTime.UtcNow, model);
                return new { result = "OK", model };
            }
            catch
            {
                return new { result = "OK", model };
            }

        }

        [HttpPost("api/convert")]
        public async Task<object> Convert([FromBody]ConvertModel model)
        {
            var amount = await _rateConverterSrv.ConvertAsync(model.From, model.To, model.Amount);
            return new { asset = model.To, amount };
        }

        [HttpGet("api/info")]
        public async Task<object> Info()
        {
            var info = await _lkk2YInfoRepository.GetInfoAsync();
            return new { startDate = 1513080000, fundsRecieved = info.FundsRecieved, fundsGoal = info.FundsGoal };
        }

    }

}
