namespace Uceme.API.Options
{
    public class AuthMessageSenderSettings
    {
        public string EmailFrom { get; internal set; }
        public string HostSmtp { get; internal set; }
        public int PortSmtp { get; internal set; }
        public string CredentialUser { get; internal set; }
        public string CredentialPassword { get; internal set; }
    }
}
