using System.Security.Cryptography;
using System.Text.Json;
using Helios;
using Helios.Data;

try
{
    var rnd = new Random();

    for (int i = 0; i < 3; i++)
    {
        using var wrapper = new ExternalAlgorithmWrapper();
        await wrapper.StartAsync();

        Console.WriteLine("==================");

        byte[] data = new byte[rnd.Next(500, 600)];
        new Random().NextBytes(data);

        var data_md5 = MD5.HashData(data);
        Console.WriteLine($"Origin Md5:\t{string.Join("", data_md5.Select(b => $"{b:x2}"))}");

        var reply_data = await wrapper.InteractAsync(data);

        var reply = JsonSerializer.Deserialize(reply_data, SourceGenerationContext.Default.ReplyData)!;

        Console.WriteLine($"Length:\t{reply.DataLength}");
        Console.WriteLine($"Md5:\t{reply.Md5}");
        foreach (var staff in reply.Staffs)
        {
            Console.WriteLine($"{staff.ID,-8}{staff.FirstName + " " + staff.LastName,-25}{staff.Email}");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}


await Task.Delay(10000);