namespace Lykke.Service.Lkk2Y_Api.Services.Smtp
{
    public interface ISmtpSenderSettings
    {
        string Host { get; }
        int Port { get; }
        string Login { get; }
        string Password { get; }
        string From { get; }
        string FromDisplayName { get; }     
        string Domain { get; }
        bool UseSsl { get; }
    }

}
