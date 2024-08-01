using System.Net.Sockets;

namespace Red.FTP.Interfaces;

public interface IFtpCommand : IDisposable
{
    void SetNetworkStream(NetworkStream stream);
    void SendCommand(string command);
    Task<string> ReadResponseAsync();
}
