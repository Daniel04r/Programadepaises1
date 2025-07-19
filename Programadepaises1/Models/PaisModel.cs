using System.Text.Json.Serialization;

namespace Programadepaises1.Models;

public class PaisModel
{
    [JsonPropertyName("name")]
    public Name Name { get; set; } = new();

    [JsonPropertyName("flags")]
    public Flags Flags { get; set; } = new();

    [JsonPropertyName("capital")]
    public List<string>? Capital { get; set; }

    [JsonPropertyName("population")]
    public long Population { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; } = string.Empty;

    // Propiedades calculadas para la UI
    public string NombreComun => Name?.Common ?? "Desconocido";
    public string BanderaUrl => Flags?.Png ?? string.Empty;
    public string CapitalTexto => Capital?.FirstOrDefault() ?? "No disponible";
    public string PoblacionTexto => Population.ToString("N0");
    public bool EsFavorito { get; set; }
}

public class Name
{
    [JsonPropertyName("common")]
    public string Common { get; set; } = string.Empty;

    [JsonPropertyName("official")]
    public string Official { get; set; } = string.Empty;
}

public class Flags
{
    [JsonPropertyName("png")]
    public string Png { get; set; } = string.Empty;

    [JsonPropertyName("svg")]
    public string Svg { get; set; } = string.Empty;
}
