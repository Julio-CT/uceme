namespace Uceme.Model.DataContracts
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class AppointmentHoursResponse
    {
        [JsonPropertyName("hours")]
        public IEnumerable<string> Hours { get; set; }
    }
}
