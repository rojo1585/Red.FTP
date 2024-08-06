using System.Net.Sockets;

namespace Red.FTP.Interfaces;

public interface IConnection : IDisposable
{
    Task<NetworkStream> CreateConnectionAsync(CancellationToken cancellationToken = default);
}
