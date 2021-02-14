namespace Uceme.Model.DataContracts
{
    using System.Text.Json.Serialization;

    public class AppointmentRequest
    {
        [JsonPropertyName("weekDay")]
        public string WeekDay { get; set; }

        [JsonPropertyName("hospitalId")]
        public string HospitalId { get; set; }

        [JsonPropertyName("day")]
        public string Day { get; set; }

        [JsonPropertyName("hour")]
        public string Hour { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("extraInfo")]
        public string ExtraInfo { get; set; }
    }
}
