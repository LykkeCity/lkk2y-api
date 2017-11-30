using System.Threading.Tasks;

namespace Lykke.Service.Lkk2Y_Api.Core
{
    public interface IRateConverterClient
    {
        Task<double> GetRateAsync(string assetFrom, string assetTo);
    }
}