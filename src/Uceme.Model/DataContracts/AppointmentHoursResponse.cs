using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Uceme.Model.DataContracts;

public class AppointmentHoursResponse
{
    [JsonPropertyName("hours")]
    public IEnumerable<string>? Hours { get; set; }
}
