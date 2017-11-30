using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Flurl.Http;
using Lykke.Service.Lkk2Y_Api.Core;

namespace Lykke.Service.Lkk2Y_Api.Services
{


    public class RateConverterService
    {


        public const string LKK2YAsset = "LKK2Y";

        public const string CHFAsset = "CHF";


        private readonly IRateConverterClient _rateConverterClient;

        private readonly ILkk2yToChf _lkk2YToChf;


        public RateConverterService(IRateConverterClient rateConverterClient, ILkk2yToChf lkk2YToChf)
        {
            _rateConverterClient = rateConverterClient;
            _lkk2YToChf = lkk2YToChf;
        }


        private async Task<double> ConvertFromLKK2yAsync(string assetTo, double volume)
        {
            var chfRate = _lkk2YToChf.GetRate(volume);

            if (assetTo == CHFAsset)
                return chfRate;

            return await _rateConverterClient.GetRateAsync(CHFAsset, assetTo);

        }

        public async Task<double> ConvertAsync(string assetFrom, string assetTo, double volume)
        {

            if (assetFrom == LKK2YAsset)
                return await ConvertFromLKK2yAsync(assetTo, volume);

            return 1;
        }

    }
}