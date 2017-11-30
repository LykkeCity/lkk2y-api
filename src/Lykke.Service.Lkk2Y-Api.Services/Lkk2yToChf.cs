using Lykke.Service.Lkk2Y_Api.Core;

namespace Lykke.Service.Lkk2Y_Api.Services
{
    public class Lkk2yToChf : ILkk2yToChf
    {
        public double GetRate(double volume)
        {
            return 0.21;
        }
    }
}