using System.Threading.Tasks;

namespace Lykke.Service.Lkk2Y_Api.Core
{
    public interface IEmailVerificationTokensRepository
    {
        
        Task<string> GetTokenAsync(string email);
        
        Task<string> VerifyTokenAsync(string token);
        
    }
    
}
