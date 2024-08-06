using Red.FTP.Exemptions;
using Red.FTP.Handler;
using Red.FTP.Interfaces;
using Red.FTP.Models;
using static Red.FTP.Models.Authentication;

namespace Red.FTP.Modules;

internal class FtpAuthenticator(IFtpCommand _commands) : IFtpAuthenticator
{
    private BasicFtpCredentials? _auth;

    public void SetCredentials(string user, string password)
    {
        if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("User and password cannot be null or empty.");

        _auth = new BasicFtpCredentials(user, password);
    }

    public async Task<FtpResponse> AuthenticateAsync(CancellationToken cancellationToken)
    {
        if (_auth is null or { Password: null, User: null })
            throw new InvalidOperationException("Credentials not set.");

        _commands.SendCommand($"USER {_auth.User}");
        await _commands.ReadResponseAsync(cancellationToken);
        _commands.SendCommand($"PASS {_auth.Password}");
        string response = await _commands.ReadResponseAsync(cancellationToken);

        var (statusCode, description) = FtpStatusCodes.GetStatusCodeAndMessage(response);

        return new FtpResponse(statusCode, description);
    }

    public static void IsLogin(bool isLogin)
    {
        if (!isLogin)
            throw new FtpAuthenticationException("User not logged in");
    }
}
