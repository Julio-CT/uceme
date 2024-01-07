namespace Uceme.Model.Settings
{
    using System.Diagnostics.CodeAnalysis;

    public class AuthMessageSenderSettings
    {
        public string? EmailFrom { get; set; }

        [DisallowNull]
        public string? HostSmtp { get; set; }

        public int PortSmtp { get; set; }

        public string? CredentialUser { get; set; }

        public string? CredentialPassword { get; set; }
    }
}
