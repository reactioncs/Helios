using Helios;

using var wrapper = new PythonProcessHelper();

wrapper.Start();
Console.WriteLine("Start()");

try
{
    byte[] bytes = new byte[100];
    var reply = await wrapper.DoAnalysisAsync(bytes);

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


Console.WriteLine("end");