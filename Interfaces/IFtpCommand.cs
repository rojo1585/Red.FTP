using System.Net.Security;
using System.Net.Sockets;

namespace Red.FTP.Interfaces;

public interface IFtpCommand : IDisposable
{
    void SetNetworkStream(NetworkStream stream);
    void SetNetworkStream(SslStream stream);
    void SendCommand(string command);
    Task<string> ReadResponseAsync(CancellationToken cancellationToken = default);
}
