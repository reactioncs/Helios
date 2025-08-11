using System.Security.Cryptography;
using System.Text.Json;
using Helios;
using Helios.Data;

using var wrapper = new ExternalAlgorithmWrapper();

await wrapper.StartAsync();
Console.WriteLine("python start");

try
{
    byte[] data = new byte[500];
    new Random().NextBytes(data);

    var data_md5 = MD5.HashData(data);
    Console.WriteLine($"Origin Md5:\t{string.Join("", data_md5.Select(b => $"{b:x2}"))}");

    var reply_data = await wrapper.InteractAsync(data);

    var reply = JsonSerializer.Deserialize(reply_data, SourceGenerationContext.Default.ReplyData)!;

    Console.WriteLine($"Length:\t{reply.DataLength}");
    Console.WriteLine($"Md5:\t{reply.Md5}");
    foreach (var staff in reply.Staffs)
    {
        Console.WriteLine($"{staff.ID}\t{staff.FirstName} {staff.LastName}\t{staff.Email}");
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
