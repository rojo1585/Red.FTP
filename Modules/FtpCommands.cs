using Red.FTP.Interfaces;
using System.Net.Sockets;
using System.Text;

namespace Red.FTP.Modules;

internal class FtpCommands : IFtpCommand
{
    private bool _disposed = false;
    private NetworkStream? ControlStream { get; set; }

    public void SetNetworkStream(NetworkStream stream)
        => ControlStream = stream;

    public async Task<string> ReadResponseAsync()
    {
        ArgumentNullException.ThrowIfNull(ControlStream);

        StringBuilder response = new();
        byte[] buffer = new byte[1024];
        int bytesRead;

        do
        {
            bytesRead = await ControlStream.ReadAsync(buffer);
            response.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
        } while (bytesRead > 0 && !response.ToString().EndsWith("\r\n"));

        return response.ToString();
    }
    public void SendCommand(string command)
    {
        byte[] data = Encoding.ASCII.GetBytes($"{command}\r\n");
        ControlStream?.Write(data, 0, data.Length);
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
                ControlStream?.Dispose();
            }
            _disposed = true;
        }
    }


}
