using System.Text.Json.Serialization;

namespace Helios.Data;

[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(RelpyData))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}