using System.Security.Cryptography;
using Helios;

using var wrapper = new PythonAlgorithmWrapper();

wrapper.Start();
Console.WriteLine("python start");

try
{
    byte[] data = new byte[100];
    new Random().NextBytes(data);

    var data_md5 = MD5.HashData(data);
    Console.WriteLine($"Origin Md5:\t{string.Join("", data_md5.Select(b => $"{b:x2}"))}");

    var reply = await wrapper.RunAsync(data);

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
