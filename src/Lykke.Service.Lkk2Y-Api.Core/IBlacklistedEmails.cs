using System.Threading.Tasks;

namespace Lykke.Service.Lkk2Y_Api.Core
{
    public interface IBlacklistedEmailsRepository
    {
        Task<bool> IsBlacklistedAsync(string email);
    }
}
