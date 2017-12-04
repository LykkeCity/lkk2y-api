using System.Threading.Tasks;
using Lykke.Service.Lkk2Y_Api.Core.Smtp;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Lykke.Service.Lkk2Y_Api.Services.Smtp
{
    public class SmtpSender : ISmtpSender
    {
        private readonly ISmtpSenderSettings _smtpSenderSettings;

        public SmtpSender(ISmtpSenderSettings smtpSenderSettings)
        {
            _smtpSenderSettings = smtpSenderSettings;
        }


        private SecureSocketOptions GetSecureSocketOptions()
        {
            return _smtpSenderSettings.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.None;
        }
        
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_smtpSenderSettings.FromDisplayName, _smtpSenderSettings.From));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;

            var bodyPart = new TextPart("html") { Text = body };

            emailMessage.Body = bodyPart;


            using (var client = new SmtpClient())
            {
                client.LocalDomain = _smtpSenderSettings.Domain;

                await client.ConnectAsync(_smtpSenderSettings.Host, _smtpSenderSettings.Port, GetSecureSocketOptions()).ConfigureAwait(false);
                await client.AuthenticateAsync(_smtpSenderSettings.Login, _smtpSenderSettings.Password);

                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
            
        }
        
    }
}
