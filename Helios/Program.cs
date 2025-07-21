using Helios;

using var wrapper = new PythonAlgorithmWrapper();

wrapper.Start();
Console.WriteLine("python start");

try
{
    byte[] bytes = new byte[100];
    var reply = await wrapper.RunAsync(bytes);

    Console.WriteLine($"Length: {reply.DataLength}");
    foreach (var staff in reply.Staffs)
    {
        Console.WriteLine($"{staff.ID}\t{staff.FirstName} {staff.LastName}\t{staff.Email}");
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
