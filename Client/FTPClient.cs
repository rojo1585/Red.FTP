using Red.FTP.Helpers;
using Red.FTP.Interfaces;
using Red.FTP.Models;
using System.Net.Sockets;
using static Red.FTP.Models.Authentication;

namespace Red.FTP.Client;

public class FtpClient(IFtpCommand _commands, IFtpDataTransfer _ftpDataTransfer) : IFtpClient
{
    private TcpClient? _controlClient;
    private BasicFtpCredentials? _auth;
    private bool _disposed = false;


    public void SetCredentials(string user, string password) =>
        _auth = new BasicFtpCredentials(user, password);

    public async Task<string> CreateConnectionAsync(string host, int port = 21)
    {
        _controlClient = new TcpClient();
        await _controlClient.ConnectAsync(host, port);
        _commands.SetNetworkStream(_controlClient.GetStream());
        return await _commands.ReadResponseAsync();
    }

    public async Task<string> AuthAsync()
    {
        if (_auth is null or { Password: null, User: null }) throw new InvalidOperationException("Credentials not set.");
        _commands.SendCommand($"USER {_auth.User}");
        await _commands.ReadResponseAsync();
        _commands.SendCommand($"PASS {_auth.Password}");
        return await _commands.ReadResponseAsync();
    }

    public async Task<IEnumerable<FtpFile>> GetFilesAsync(string path)
    {
        var (ip, port) = await GetPasiveConnection();
        return await _ftpDataTransfer.GetFilesAsync(path, ip, port);
    }

    private async Task<(string ip, int port)> GetPasiveConnection()
    {
        _commands.SendCommand("PASV");
        string response = await _commands.ReadResponseAsync();
        return FtpResponseParser.ParsePasiveResponse(response);
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
                _controlClient?.Dispose();
            }
            _disposed = true;
        }
    }
}