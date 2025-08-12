using System.Text.Json.Serialization;

namespace Helios.Data;

[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(InputData))]
[JsonSerializable(typeof(ReplyData))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}