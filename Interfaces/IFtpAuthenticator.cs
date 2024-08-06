using Red.FTP.Models;

namespace Red.FTP.Interfaces
{
    public interface IFtpAuthenticator
    {
        void SetCredentials(string user, string password);
        Task<FtpResponse> AuthenticateAsync(CancellationToken cancellationToken);
    }
}
