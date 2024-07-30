using Red.FTP.Interfaces;
using System.Net.Sockets;
using System.Text;
using static Red.FTP.Models.Authentication;

namespace Red.FTP.Services;

public class FtpClient(string _host) : IFtpClient
{

    private TcpClient? _controlClient;
    private NetworkStream? _controlStream;
    private BasicFtpCredentials? _auth;
    private bool _disposed = false;

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
                _controlClient?.Dispose();
            }
            _disposed = true;
        }
    }

    public void SetCredentials(string user, string password) =>
        _auth = new BasicFtpCredentials(user, password);

    public async Task<string> CreateConnectionAsync()
    {
        _controlClient = new TcpClient();
        await _controlClient.ConnectAsync(_host, 21);
        _controlStream = _controlClient.GetStream();
        return await ReadResponseAsync();
    }

    public async Task<string> AuthAsync()
    {
        if (_auth == null) throw new InvalidOperationException("Credentials not set.");
        SendCommand($"USER {_auth.User}");
        await ReadResponseAsync();
        SendCommand($"PASS {_auth.Password}");
        return await ReadResponseAsync();
    }

    public void SendCommand(string command)
    {
        byte[] data = Encoding.ASCII.GetBytes($"{command}\r\n");
        _controlStream?.Write(data, 0, data.Length);
    }

    public async Task<string> ReadResponseAsync()
    {
        ArgumentNullException.ThrowIfNull(_controlStream);

        StringBuilder response = new();
        byte[] buffer = new byte[1024];
        int bytesRead;

        do
        {
            bytesRead = await _controlStream.ReadAsync(buffer);
            response.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
        } while (bytesRead > 0 && !response.ToString().EndsWith("\r\n"));

        return response.ToString();
    }
}