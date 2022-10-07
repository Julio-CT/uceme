namespace Uceme.Model.DataContracts
{
    using System.Text.Json.Serialization;

    public class EmailMessage
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("loaded")]
        public bool Loaded { get; set; }
    }
}
