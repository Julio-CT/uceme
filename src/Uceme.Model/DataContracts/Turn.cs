using System.Text.Json.Serialization;

namespace Uceme.Model.DataContracts;

public class Turn
{
    [JsonPropertyName("idTurno")]
    public int IdTurno { get; set; }

    [JsonPropertyName("dia")]
    public int Dia { get; set; }

    [JsonPropertyName("inicio")]
    public string? Inicio { get; set; }

    [JsonPropertyName("fin")]
    public string? Fin { get; set; }

    [JsonPropertyName("paralelas")]
    public int Paralelas { get; set; }

    [JsonPropertyName("porhora")]
    public int Porhora { get; set; }

    [JsonPropertyName("idHospital")]
    public int IdHospital { get; set; }
}
