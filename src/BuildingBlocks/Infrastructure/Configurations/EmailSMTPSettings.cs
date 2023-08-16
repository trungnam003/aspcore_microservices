using Contracts.Configurations;
#nullable disable

namespace Infrastructure.Configurations
{
    public class EmailSMTPSettings : IEmailSMTPSettings
    {
        public string DisplayName { get; set; }
        public bool EnableVerification { get; set; }
        public string From { get; set; }
        public string SMTPServer { get; set; }
        public bool UseSsl { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
