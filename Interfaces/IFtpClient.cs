using Red.FTP.Models;

namespace Red.FTP.Interfaces;

public interface IFtpClient : IDisposable
{
    Task<FtpResponse> CreateConnectionAsync(string host, int port = 21);
    Task<FtpResponse> AuthAsync();
}
