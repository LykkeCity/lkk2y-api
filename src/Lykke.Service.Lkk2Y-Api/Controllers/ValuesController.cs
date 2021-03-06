﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly TransctionMailSender _transctionMailSender;
        private readonly IBlacklistedEmailsRepository _blacklistedEmails;


        public ValuesController(ILkk2YOrdersRepository lkk2YOrdersRepository, 
        ILkk2yInfoRepository lkk2YInfoRepository, RateConverterService rateConverterSrv, 
            TransctionMailSender transctionMailSender,
            IBlacklistedEmailsRepository blacklistedEmails)
        {
            _lkk2YOrdersRepository = lkk2YOrdersRepository;
            _lkk2YInfoRepository = lkk2YInfoRepository;
            _rateConverterSrv = rateConverterSrv;
            _transctionMailSender = transctionMailSender;
            _blacklistedEmails = blacklistedEmails;
        }

        [HttpPost("api/subscribe")]
        public object Subscribe([FromBody]SubscribeModel model)
        {
            return new { result = "OK", model }; 
        }


        private static object GetOrderResponse(OrderModel model)
        {
            return new
            {
                result = "OK",
                model
            };
        }

        [HttpPost("api/order")]
        public async Task<object> Order([FromBody] OrderModel model)
        {

            var body = await Request.BodyAsStringAsync();

            if (model == null)
            {
                Console.WriteLine("Order: !!!! Model is null !!!!!; Body=" + body);

                return NotFound();
            }

            model.Ip = this.GetIp();

            Console.WriteLine("Order:" + model.ToJson() + "; Body=" + body);

            model.Currency = model.Currency.Trim();

            model.UsdAmount = await _rateConverterSrv.ConvertAsync(RateConverterService.LKK2YAsset,
                RateConverterService.CHFAsset, model.Amount);

            if (model.UsdAmount > Lkk2YConstants.MaxOrderSize)
            {
                await _blacklistedEmails.AddAsync(model.Email);
                await _lkk2YOrdersRepository.RegisterIgnoredAsync(DateTime.UtcNow, model);    
                return GetOrderResponse(model); 
            }

            Console.WriteLine("Order with USD:" + model.ToJson());


            if (DoubleCheckers.HasDouble(model))
                return GetOrderResponse(model);


            if (await _blacklistedEmails.IsBlacklistedAsync(model.Email))
            {
                await _lkk2YOrdersRepository.RegisterIgnoredAsync(DateTime.UtcNow, model); 
                return GetOrderResponse(model);                
            }

            await _lkk2YOrdersRepository.RegisterAsync(DateTime.UtcNow, model);

            await _transctionMailSender.SenderTransactionalEmail(model.Email);

            return GetOrderResponse(model);

        }

        [HttpPost("api/convert")]
        public async Task<object> Convert([FromBody]ConvertModel model)
        {

            var body = await Request.BodyAsStringAsync();


            if (model == null){
                Console.WriteLine("Convert: !!!! Model is null !!!!!; Body: "+body);
                return NotFound();
            }

            Console.WriteLine("Convert:" + model.ToJson()+"; Body: "+body);

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
                startDate = 1513076400,
                fundsRecieved = infoTask.Result.FundsRecieved,
                fundsGoal = infoTask.Result.FundsGoal,
                fundsTotal = totalTaks.Result
            };
        }
        
        [HttpGet("api/updatetotal")]
        public async Task<object> UpdateTotal()
        {

            var total = await _lkk2YOrdersRepository.UpdateTotalAsync();
            
            return new
            {
                result = "OK",
                total
            };
            
        }
        
        
        [HttpGet("api/calc")]
        public async Task<object> CalcTotal()
        {

            var total = await _lkk2YOrdersRepository.CalcTotalAsync();
            
            return new
            {
                result = "OK",
                total
            };
            
        }
        
        [HttpGet("api/isordermade/{email}")]        
        public async Task<object> IsOrderMade(string email)
        {
            return await _lkk2YOrdersRepository.IsEmailRegistered(email);
        }

    }

}
