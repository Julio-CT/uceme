//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Uceme.Model.Models
{
    using System.Text.Json.Serialization;

    public partial class CalendarEvent
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("title")]
        public string title { get; set; }

        [JsonPropertyName("start")]
        public string start { get; set; }

        [JsonPropertyName("end")]
        public string end { get; set; }

        [JsonPropertyName("desc")]
        public string description { get; set; }
    }
}
