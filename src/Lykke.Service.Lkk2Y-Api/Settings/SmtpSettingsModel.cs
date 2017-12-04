using Lykke.Service.Lkk2Y_Api.Services.Smtp;

namespace Lykke.Service.Lkk2Y_Api.Settings
{
    public class SmtpSettingsModel : ISmtpSenderSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
        public string FromDisplayName { get; set; }
        public string Domain { get; set; }
        public bool UseSsl { get; set; }
    }
}
