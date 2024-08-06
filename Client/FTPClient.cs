using Red.FTP.Exceptions;
using Red.FTP.Handler;
using Red.FTP.Interfaces;
using Red.FTP.Models;
using Red.FTP.Modules;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace Red.FTP.Client;

public class FtpClient(IFtpCommand commands, IFtpFileTransfer ftpDataTransfer, IConnection passiveConnection, IFtpAuthenticator authenticator, bool useSsl) : IFtpClient
{
    private readonly IFtpCommand _commands = commands ?? throw new ArgumentNullException(nameof(commands));
    private readonly IFtpFileTransfer _ftpDataTransfer = ftpDataTransfer ?? throw new ArgumentNullException(nameof(ftpDataTransfer));
    private readonly IConnection _passiveConnection = passiveConnection ?? throw new ArgumentNullException(nameof(passiveConnection));
    private readonly IFtpAuthenticator _authenticator = authenticator ?? throw new ArgumentNullException(nameof(authenticator));
    private TcpClient? _controlClient;
    private SslStream? _sslStream;
    private bool _disposed = false;
    private bool isLogin = false;

    public void SetCredentials(string user, string password) => _authenticator.SetCredentials(user, password);

    public async Task<FtpResponse> CreateConnectionAsync(string host, int port = 21, CancellationToken cancellationToken = default)
    {
        try
        {
            _controlClient = new TcpClient();
            await _controlClient.ConnectAsync(host, port, cancellationToken);

            if (useSsl)
            {
                _sslStream = new SslStream(_controlClient.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                await _sslStream.AuthenticateAsClientAsync(host);
                _commands.SetNetworkStream(_sslStream);
            }
            else
            {
                _commands.SetNetworkStream(_controlClient.GetStream());
            }

            var response = await _commands.ReadResponseAsync(cancellationToken);
            var (statusCode, description) = FtpStatusCodes.GetStatusCodeAndMessage(response);
            return new FtpResponse(statusCode, description);
        }
        catch (Exception ex)
        {
            throw new FtpConnectionException("Failed to create connection", ex);
        }
    }

    public async Task<FtpResponse> AuthAsync(CancellationToken cancellationToken = default)
    {
        var response = await _authenticator.AuthenticateAsync(cancellationToken);
        if (FtpStatusCodes.IsAuthenticatedStatusCode(response.StatusCode))
            isLogin = true;

        return response;
    }

    public async Task<IEnumerable<FtpFile>> GetFilesAsync(string path, CancellationToken cancellationToken = default)
    {
        FtpAuthenticator.IsLogin(isLogin);

        var stream = await _passiveConnection.CreateConnectionAsync(cancellationToken);
        return await _ftpDataTransfer.GetFilesAsync(stream, path, cancellationToken);
    }

    public async Task DownloadAsync(string localPath, string remotePath, CancellationToken cancellationToken = default)
    {
        FtpAuthenticator.IsLogin(isLogin);

        using var stream = await _passiveConnection.CreateConnectionAsync(cancellationToken);
        await _ftpDataTransfer.DownloadFileAsync(stream, localPath, remotePath, cancellationToken);
    }
    public async Task UploadFileAsync(string localPath, string remotePath, CancellationToken cancellationToken = default)
    {
        using var stream = await _passiveConnection.CreateConnectionAsync(cancellationToken);
        await _ftpDataTransfer.UploadFileAsync(stream, localPath, remotePath, cancellationToken);
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
                _sslStream?.Dispose();
                _controlClient?.Dispose();
            }
            _disposed = true;
        }
    }

    private static bool ValidateServerCertificate(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
    {
        return sslPolicyErrors == SslPolicyErrors.None;
    }
}