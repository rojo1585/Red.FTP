namespace Red.FTP.Interfaces;

public interface IFtpClient : IDisposable
{
    Task<string> CreateConnectionAsync();
    Task<string> AuthAsync();
}
