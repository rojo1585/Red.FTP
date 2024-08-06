using Red.FTP.Interfaces;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace Red.FTP.Modules;

internal class FtpCommands : IFtpCommand
{
    private bool _disposed = false;
    private NetworkStream? _controlStream;
    private SslStream? _controlSslStream;

    public void SetNetworkStream(NetworkStream stream)
    {
        _controlStream = stream;
        _controlSslStream = null;
    }

    public void SetNetworkStream(SslStream stream)
    {
        _controlSslStream = stream;
        _controlStream = null;
    }

    public async Task<string> ReadResponseAsync(CancellationToken cancellationToken = default)
    {
        if (_controlStream == null && _controlSslStream == null)
            throw new InvalidOperationException("Control stream is not set");

        StringBuilder response = new();
        byte[] buffer = new byte[1024];
        int bytesRead;

        do
        {
            if (_controlStream != null)
                bytesRead = await _controlStream.ReadAsync(buffer.AsMemory(), cancellationToken);
            else
                bytesRead = await _controlSslStream!.ReadAsync(buffer.AsMemory(), cancellationToken);

            response.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));

            cancellationToken.ThrowIfCancellationRequested();
        } while (bytesRead > 0 && !response.ToString().EndsWith("\r\n"));

        return response.ToString();
    }

    public void SendCommand(string command)
    {
        byte[] data = Encoding.ASCII.GetBytes($"{command}\r\n");

        if (_controlStream != null)
            _controlStream.Write(data, 0, data.Length);
        else if (_controlSslStream != null)
            _controlSslStream.Write(data, 0, data.Length);
        else
            throw new InvalidOperationException("Control stream is not set");
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _controlStream?.Dispose();
                _controlSslStream?.Dispose();
            }
            _disposed = true;
        }
    }
}