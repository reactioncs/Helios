using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Json;

namespace Helios;

public class PythonProcessHelper : IDisposable
{
    private readonly string _executable;
    private readonly string _arguments;

    private Process? _process;

    public PythonProcessHelper()
    {
        _executable = "python";
        _arguments = "default_python/operation.py";
    }

    public void Start()
    {
        _process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = _executable,
                Arguments = _arguments,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            },
        };

        _process.Start();
    }

    public async Task<RelpyData> DoAnalysisAsync(byte[] data)
    {
        if (_process == null) throw new Exception("_process == null");

        var stdin = new BinaryWriter(_process.StandardInput.BaseStream);

        var header = new byte[8];
        var lengthPrefix = BitConverter.GetBytes(data.Length);
        Array.Copy(lengthPrefix, 0, header, 0, 4);

        SHA256.HashData(header);

        stdin.Write(header);
        stdin.Flush();

        stdin.Write(data);
        stdin.Flush();

        var ret = await _process.StandardOutput.ReadLineAsync();

        if (ret == "Error")
        {
            var error = await _process.StandardError.ReadLineAsync();
            throw new Exception(error);
        }

        return JsonSerializer.Deserialize<RelpyData>(ret!)!;
    }

    public void Dispose()
    {
        _process?.Dispose();
        GC.SuppressFinalize(this);
    }
}
