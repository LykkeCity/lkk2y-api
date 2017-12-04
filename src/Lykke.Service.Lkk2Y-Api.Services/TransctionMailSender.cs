using System.Threading.Tasks;
using Flurl.Http;
using Lykke.Service.Lkk2Y_Api.Core.Smtp;

namespace Lykke.Service.Lkk2Y_Api.Services
{
    public class TransctionMailSender
    {
        private readonly string _templateUrl;
        private readonly ISmtpSender _smtpSender;


        public TransctionMailSender(string templateUrl, ISmtpSender smtpSender)
        {
            _templateUrl = templateUrl;
            _smtpSender = smtpSender;
        }

        private async Task<string> LoadTemplateAsync()
        {
            return await _templateUrl.GetStringAsync();
        }

        private const string Subject = "LKK2Y whitelisting";

        public async Task SenderTransactionalEmail(string email)
        {

            var template = await LoadTemplateAsync();

            await _smtpSender.SendEmailAsync(email, Subject, template);

        }
        
    }
}
