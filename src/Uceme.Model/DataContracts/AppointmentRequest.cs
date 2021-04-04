namespace Uceme.Model.DataContracts
{
    using System.Text.Json.Serialization;

    public class AppointmentRequest
    {
        [JsonPropertyName("weekDay")]
        public int WeekDay { get; set; }

        [JsonPropertyName("hospitalId")]
        public int HospitalId { get; set; }

        [JsonPropertyName("day")]
        public int Day { get; set; }

        [JsonPropertyName("month")]
        public int Month { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }

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
