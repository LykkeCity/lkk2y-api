using Lykke.Service.Lkk2Y_Api.Core.Settings.ServiceSettings;
using Lykke.Service.Lkk2Y_Api.Core.Settings.SlackNotifications;

namespace Lykke.Service.Lkk2Y_Api.Core.Settings
{
    public class AppSettings
    {
        public Lkk2Y_ApiSettings Lkk2Y_ApiService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }
}
