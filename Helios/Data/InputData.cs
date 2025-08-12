using System.Text.Json.Serialization;

namespace Helios.Data;

public class InputData
{
    [JsonPropertyName("weather_measures")]
    public List<WeatherMeasure> WeatherMeasures { get; set; } = [];
}

public class WeatherMeasure
{
    [JsonPropertyName("id")]
    public int ID { get; set; }
    [JsonPropertyName("city")]
    public string City { get; set; } = string.Empty;
    [JsonPropertyName("temperature")]
    public float Temperature { get; set; }
}