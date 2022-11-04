namespace Uceme.Model.DataContracts
{
    using System.Text.Json.Serialization;

    public class AppointmentHoursRequest
    {
        [JsonPropertyName("weekDay")]
        public int WeekDay { get; set; }

        [JsonPropertyName("hospitalId")]
        public string? HospitalId { get; set; }

        [JsonPropertyName("day")]
        public int Day { get; set; }

        [JsonPropertyName("month")]
        public int Month { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }
    }
}
