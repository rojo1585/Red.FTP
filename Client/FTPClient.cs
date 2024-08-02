using Red.FTP.Exemptions;
using Red.FTP.Handler;
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
    private bool isLogin = false;

    public void SetCredentials(string user, string password) =>
        _auth = new BasicFtpCredentials(user, password);

    public async Task<FtpResponse> CreateConnectionAsync(string host, int port = 21)
    {
        _controlClient = new TcpClient();
        await _controlClient.ConnectAsync(host, port);
        _commands.SetNetworkStream(_controlClient.GetStream());
        var response = await _commands.ReadResponseAsync();

        var (statusCode, description) = FtpStatusCodes.GetStatusCodeAndMessage(response);
        return new(statusCode, description);
    }

    public async Task<FtpResponse> AuthAsync()
    {
        if (_auth is null or { Password: null, User: null }) throw new InvalidOperationException("Credentials not set.");

        _commands.SendCommand($"USER {_auth.User}");
        await _commands.ReadResponseAsync();
        _commands.SendCommand($"PASS {_auth.Password}");
        string response = await _commands.ReadResponseAsync();

        var (statusCode, description) = FtpStatusCodes.GetStatusCodeAndMessage(response);

        if (FtpStatusCodes.IsAuthenticatedStatusCode(statusCode))
        {
            isLogin = true;
        }

        return new(statusCode, description);
    }

    public async Task<IEnumerable<FtpFile>> GetFilesAsync(string path)
    {
        if (!isLogin)
            throw new FtpAuthenticationException("User not login");

        await CreatePasiveConnection();

        return await _ftpDataTransfer.GetFilesAsync(path);
    }

    public async Task DownloadAsync(string localPath, string remotePath)
    {
        await CreatePasiveConnection();
        await _ftpDataTransfer.DownloadFileAsync(localPath, remotePath);
    }
    private async Task CreatePasiveConnection()
    {
        _commands.SendCommand("PASV");
        string response = await _commands.ReadResponseAsync();
        var (ip, port) = FtpResponseParser.ParsePasiveResponse(response);
        await _ftpDataTransfer.CreateDataClient(ip, port);
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