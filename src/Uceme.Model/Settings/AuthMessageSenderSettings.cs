namespace Uceme.Model.Settings
{
    public class AuthMessageSenderSettings
    {
        public string? EmailFrom { get; set; }

        public string? HostSmtp { get; set; }

        public int PortSmtp { get; set; }

        public string? CredentialUser { get; set; }

        public string? CredentialPassword { get; set; }
    }
}
