using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Flurl.Http;
using Lykke.Service.Lkk2Y_Api.Core;

namespace Lykke.Service.Lkk2Y_Api.Services
{
    public class RateConverterClient : IRateConverterClient
    {

        private string _url;

        public RateConverterClient(string url)
        {
            _url = url.AddLastSymbolIfNotExists('/');
        }

        private Dictionary<string, double> _cache = new Dictionary<string, double>();


        private static string GetCaheKey(string assetFrom, string assetTo)
        {
            return assetFrom + assetTo;
        }


        private const double DefalutAssetPrice = 1;


        private double GetFromCache(string assetFrom, string assetTo){

            var key = GetCaheKey(assetFrom, assetTo);

            lock(_cache){
                if (_cache.ContainsKey(key))
                    return _cache[key];
            }


            return DefalutAssetPrice;

        }


        private void AddToCache(string assetFrom, string assetTo, double value){

            var key = GetCaheKey(assetFrom, assetTo);

            lock(_cache){
                if (_cache.ContainsKey(key))
                    _cache[key] = value;
                    else
                    _cache.Add(key, value);
            }

        }


        public async Task<double> GetRateAsync(string assetFrom, string assetTo)
        {
            var url = $"{_url}api/RateCalculator/GetAmountInBase/{assetFrom}/{assetTo}/1";

            try
            {
                var httpResult = await url.PostStringAsync("").ReceiveString();

                var value = httpResult.ParseAnyDoubleOrDefault(DefalutAssetPrice);

                AddToCache(assetFrom, assetTo, value);

                return value;
            }
            catch
            {
                return GetFromCache(assetFrom, assetTo);
            }
        }
    }
}