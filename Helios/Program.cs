using System.Security.Cryptography;
using System.Text.Json;
using Helios;
using Helios.Data;

try
{
    using var wrapper = new ExternalAlgorithmWrapper();
    await wrapper.StartAsync();

    byte[] inputdata = JsonSerializer.SerializeToUtf8Bytes(InputGenerator.Generate(), SourceGenerationContext.Default.InputData);

    var inputdata_md5 = MD5.HashData(inputdata);
    Console.WriteLine($"Inputdata: Length:{inputdata.Length / 1024}KB, Md5:{string.Join("", inputdata_md5.Select(b => $"{b:x2}"))}");

    var reply_data = await wrapper.InteractAsync(inputdata);

    var reply = JsonSerializer.Deserialize(reply_data, SourceGenerationContext.Default.ReplyData)!;

    Console.WriteLine($"Md5:{reply.Md5}");
    foreach (var summary in reply.Summaries)
    {
        Console.WriteLine($"{summary.City,-15}{summary.AverageTemperature:F1}");
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
