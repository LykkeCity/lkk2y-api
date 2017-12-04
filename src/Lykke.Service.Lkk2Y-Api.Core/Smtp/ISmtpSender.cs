using System.Threading.Tasks;

namespace Lykke.Service.Lkk2Y_Api.Core.Smtp
{
    public interface ISmtpSender
    {
        
        Task SendEmailAsync(string email, string subject, string body);
        
    }
    
}
