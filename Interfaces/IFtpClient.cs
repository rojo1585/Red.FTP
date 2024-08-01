namespace Red.FTP.Interfaces;

public interface IFtpClient : IDisposable
{
    Task<string> CreateConnectionAsync(string host, int port = 21);
    Task<string> AuthAsync();
}
