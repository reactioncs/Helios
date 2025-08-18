using System.Diagnostics;
using System.Text;

namespace Helios;

public class ExternalAlgorithmWrapper(string executable, string arguments) : IDisposable
{
    private readonly string _executable = executable;
    private readonly string _arguments = arguments;

    private readonly byte[] _magicNumberError = [0xcd, 0xbc, 0xd0, 0xac];

    private Process? _process;

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

    public async Task<byte[]> InteractAsync(byte[] data, CancellationToken cancellationToken)
    {
        if (_process == null) throw new Exception("Start the process before interact.");

        var header = new byte[8];
        var lengthPrefix = BitConverter.GetBytes(data.Length);
        Array.Copy(lengthPrefix, 0, header, 0, 4);

        await _process.StandardInput.BaseStream.WriteAsync(header, cancellationToken);
        await _process.StandardInput.BaseStream.FlushAsync(cancellationToken);

        await _process.StandardInput.BaseStream.WriteAsync(data, cancellationToken);
        await _process.StandardInput.BaseStream.FlushAsync(cancellationToken);

        var replyHeader = new byte[8];
        await _process.StandardOutput.BaseStream.ReadExactlyAsync(replyHeader.AsMemory(0, 8), cancellationToken);

        var replyLength = BitConverter.ToInt32(replyHeader.AsSpan(0, 4));
        if (replyLength == 0) return [];

        var replyData = new byte[replyLength];
        await _process.StandardOutput.BaseStream.ReadExactlyAsync(replyData.AsMemory(0, replyLength), cancellationToken);

        if (replyLength >= 4 && replyData.AsSpan(0, 4).SequenceEqual(_magicNumberError))
        {
            var errorHeader = new byte[8];
            await _process.StandardError.BaseStream.ReadExactlyAsync(errorHeader.AsMemory(0, 8), cancellationToken);

            var errorLength = BitConverter.ToInt32(errorHeader.AsSpan(0, 4));
            var errorData = new byte[errorLength];
            await _process.StandardError.BaseStream.ReadExactlyAsync(errorData.AsMemory(0, errorLength), cancellationToken);

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
