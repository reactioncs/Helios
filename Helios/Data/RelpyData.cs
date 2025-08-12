using System.Text.Json.Serialization;

namespace Helios.Data;

public class ReplyData
{
    [JsonPropertyName("md5")]
    public string Md5 { get; set; } = string.Empty;
    [JsonPropertyName("measure_count")]
    public int MeasureCount { get; set; }
    [JsonPropertyName("summaries")]
    public List<Summary> Summaries { get; set; } = [];
}

public class Summary
{
    [JsonPropertyName("city")]
    public string City { get; set; } = string.Empty;
    [JsonPropertyName("average_temperature")]
    public float AverageTemperature { get; set; }
}