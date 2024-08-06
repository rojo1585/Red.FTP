using Red.FTP.Models;

namespace Red.FTP.Interfaces;

public interface IFtpClient : IDisposable
{
    Task<FtpResponse> CreateConnectionAsync(string host, int port = 21, CancellationToken cancellationToken = default);
    Task<FtpResponse> AuthAsync(CancellationToken cancellationToken = default);
}
