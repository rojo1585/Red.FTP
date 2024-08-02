using Red.FTP.Exemptions;
using Red.FTP.Handler;
using Red.FTP.Interfaces;
using Red.FTP.Models;
using Red.FTP.Modules;
using System.Net.Sockets;

namespace Red.FTP.Client;

public class FtpClient(IFtpCommand _commands, IFtpDataTransfer _ftpDataTransfer) : IFtpClient
{
    private TcpClient? _controlClient;
    private readonly FtpAuthenticator _authenticator = new(_commands);
    private readonly FtpPassiveConnection _passiveConnection = new(_commands, _ftpDataTransfer);
    private bool _disposed = false;
    private bool isLogin = false;

    public void SetCredentials(string user, string password) =>
        _authenticator.SetCredentials(user, password);

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
        var response = await _authenticator.AuthenticateAsync();
        if (FtpStatusCodes.IsAuthenticatedStatusCode(response.StatusCode))
            isLogin = true;

        return response;
    }

    public async Task<IEnumerable<FtpFile>> GetFilesAsync(string path)
    {
        if (!isLogin)
            throw new FtpAuthenticationException("User not login");

        await _passiveConnection.CreateConnectionAsync();

        return await _ftpDataTransfer.GetFilesAsync(path);
    }

    public async Task DownloadAsync(string localPath, string remotePath)
    {
        await _passiveConnection.CreateConnectionAsync();
        await _ftpDataTransfer.DownloadFileAsync(localPath, remotePath);
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