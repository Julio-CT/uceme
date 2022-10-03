namespace Uceme.Model.Settings
{
    public class TokenSettings
    {
        public string? Audience { get; set; }

        public string? Authority { get; set; }

        public bool RequireHttpsMetadata { get; set; }

        public string? AudienceAlt { get; set; }

        public string? AuthorityAlt { get; set; }

        public bool RequireHttpsMetadataAlt { get; set; }
    }
}
