using Red.FTP.Helpers;
using Red.FTP.Interfaces;
using System.Net.Sockets;

namespace Red.FTP.Handler;

internal class FtpPassiveConnection(IFtpCommand commands) : IConnection
{
    private TcpClient? passiveClient;
    private bool _disposed = false;
    public async Task<NetworkStream> CreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        commands.SendCommand("PASV");
        string response = await commands.ReadResponseAsync(cancellationToken);
        var (ip, port) = FtpResponseParser.ParsePasiveResponse(response);
        return await CreateDataClient(ip, port, cancellationToken);
    }
    private async Task<NetworkStream> CreateDataClient(string ip, int port, CancellationToken cancellationToken = default)
    {
        passiveClient = new();
        await passiveClient.ConnectAsync(ip, port, cancellationToken);
        return passiveClient.GetStream();
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
                passiveClient?.Close();
                passiveClient?.Dispose();
            }
            _disposed = true;
        }
    }
}
