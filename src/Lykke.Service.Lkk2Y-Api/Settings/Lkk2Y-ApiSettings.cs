namespace Lykke.Service.Lkk2Y_Api.Settings
{
    public class Lkk2Y_ApiSettings
    {
        public DbSettings Db { get; set; }
        
        public SmtpSettingsModel SmtpSettings { get; set; }
                
        public string EmailTemplateUrl { get; set; }

        public string RateConverterUrl { get; set; }

    }
}
