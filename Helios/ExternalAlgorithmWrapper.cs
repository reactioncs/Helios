using System.Diagnostics;
using System.Text;

namespace Helios;

public class ExternalAlgorithmWrapper : IDisposable
{
    private readonly string _executable;
    private readonly string _arguments;

    private readonly byte[] _magicError = [0xcd, 0xbc, 0xd0, 0xac];

    private Process? _process;

    public ExternalAlgorithmWrapper()
    {
        _executable = "python";
        _arguments = "default_python/operation.py";
    }

    public Task StartAsync()
    {
        return Task.Run(() => Start());
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

    public async Task<byte[]> InteractAsync(byte[] data)
    {
        if (_process == null) throw new Exception("Start the process before interact.");

        var header = new byte[8];
        var lengthPrefix = BitConverter.GetBytes(data.Length);
        Array.Copy(lengthPrefix, 0, header, 0, 4);

        await _process.StandardInput.BaseStream.WriteAsync(header);
        await _process.StandardInput.FlushAsync();

        await _process.StandardInput.BaseStream.WriteAsync(data);
        await _process.StandardInput.FlushAsync();

        var replyHeader = new byte[8];
        if (replyHeader.Length != await _process.StandardOutput.BaseStream.ReadAsync(replyHeader.AsMemory(0, replyHeader.Length)))
            throw new Exception("Read reply header failed.");

        var replyLength = BitConverter.ToInt32(replyHeader.AsSpan(0, 4));
        var replyData = new byte[replyLength];
        if (replyData.Length != await _process.StandardOutput.BaseStream.ReadAsync(replyData.AsMemory(0, replyData.Length)))
            throw new Exception("Read reply data failed.");


        if (replyData.Length >= 4 && replyData.AsSpan(0, 4).SequenceEqual(_magicError))
        {
            var errorHeader = new byte[8];
            if (errorHeader.Length != await _process.StandardError.BaseStream.ReadAsync(errorHeader.AsMemory(0, errorHeader.Length)))
                throw new Exception("Read error header failed.");

            var errorLength = BitConverter.ToInt32(errorHeader.AsSpan(0, 4));
            var errorData = new byte[errorLength];
            if (errorData.Length != await _process.StandardError.BaseStream.ReadAsync(errorData.AsMemory(0, errorData.Length)))
                throw new Exception("Read error data failed.");

            string errorMessage = Encoding.UTF8.GetString(errorData);

            throw new Exception(errorMessage);
        }

        return replyData;
    }

    public void Dispose()
    {
        _process?.Close();
        _process?.Dispose();
        GC.SuppressFinalize(this);
    }
}
