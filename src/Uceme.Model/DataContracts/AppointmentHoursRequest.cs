namespace Uceme.Model.DataContracts
{
    using System.Text.Json.Serialization;

    public class AppointmentHoursRequest
    {
        [JsonPropertyName("weekDay")]
        public string WeekDay { get; set; }

        [JsonPropertyName("hospitalId")]
        public string HospitalId { get; set; }

        [JsonPropertyName("day")]
        public string Day { get; set; }

        [JsonPropertyName("month")]
        public string Month { get; set; }

        [JsonPropertyName("year")]
        public string Year { get; set; }
    }
}
