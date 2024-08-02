using Red.FTP.Handler;
using Red.FTP.Interfaces;
using Red.FTP.Models;
using static Red.FTP.Models.Authentication;

namespace Red.FTP.Modules;

internal class FtpAuthenticator(IFtpCommand commands)
{
    private BasicFtpCredentials? _auth;

    public void SetCredentials(string user, string password)
    {
        if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("User and password cannot be null or empty.");

        _auth = new BasicFtpCredentials(user, password);
    }

    public async Task<FtpResponse> AuthenticateAsync()
    {
        if (_auth is null or { Password: null, User: null })
            throw new InvalidOperationException("Credentials not set.");

        commands.SendCommand($"USER {_auth.User}");
        await commands.ReadResponseAsync();
        commands.SendCommand($"PASS {_auth.Password}");
        string response = await commands.ReadResponseAsync();

        var (statusCode, description) = FtpStatusCodes.GetStatusCodeAndMessage(response);

        return new FtpResponse(statusCode, description);
    }
}
