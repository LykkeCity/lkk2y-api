using System.Threading.Tasks;

namespace Lykke.Service.Lkk2Y_Api.Core
{
    public interface IBlacklistedEmails
    {
        Task<bool> IsBlacklistedAsync(string email);
    }
}
