using System.Threading.Tasks;

namespace Lykke.Service.Lkk2Y_Api.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();
    }
}