using System.Diagnostics;
using System.Text;

namespace Helios;

public class ExternalAlgorithmWrapper : IDisposable
{
    private readonly string _executable;
    private readonly string _arguments;

    private readonly byte[] _magicNumberError = [0xcd, 0xbc, 0xd0, 0xac];

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
        await _process.StandardInput.BaseStream.FlushAsync();

        await _process.StandardInput.BaseStream.WriteAsync(data);
        await _process.StandardInput.BaseStream.FlushAsync();

        var replyHeader = new byte[8];
        if (8 != await _process.StandardOutput.BaseStream.ReadAsync(replyHeader.AsMemory(0, 8)))
            throw new Exception("Read reply header failed.");

        var replyLength = BitConverter.ToInt32(replyHeader.AsSpan(0, 4));
        var replyData = new byte[replyLength];
        if (replyLength != await _process.StandardOutput.BaseStream.ReadAsync(replyData.AsMemory(0, replyLength)))
            throw new Exception("Read reply data failed.");


        if (replyLength >= 4 && replyData.AsSpan(0, 4).SequenceEqual(_magicNumberError))
        {
            var errorHeader = new byte[8];
            if (8 != await _process.StandardError.BaseStream.ReadAsync(errorHeader.AsMemory(0, 8)))
                throw new Exception("Read error header failed.");

            var errorLength = BitConverter.ToInt32(errorHeader.AsSpan(0, 4));
            var errorData = new byte[errorLength];
            if (errorLength != await _process.StandardError.BaseStream.ReadAsync(errorData.AsMemory(0, errorLength)))
                throw new Exception("Read error data failed.");

            string errorMessage = Encoding.UTF8.GetString(errorData);
            throw new Exception(errorMessage);
        }

        return replyData;
    }

    public void Dispose()
    {
        _process?.Kill();
        _process?.Dispose();
        GC.SuppressFinalize(this);
    }
}
