using System.Text.Json.Serialization;

namespace Helios.Data;

public class RelpyData
{
    [JsonPropertyName("data_length")]
    public int DataLength { get; set; }
    [JsonPropertyName("md5")]
    public string Md5 { get; set; } = string.Empty;
    [JsonPropertyName("staffs")]
    public List<Staff> Staffs { get; set; } = [];
}

public class Staff
{
    [JsonPropertyName("id")]
    public int ID { get; set; }
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = string.Empty;
    [JsonPropertyName("last_name")]
    public string LastName { get; set; } = string.Empty;
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}