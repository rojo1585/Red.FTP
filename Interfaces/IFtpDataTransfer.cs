using Red.FTP.Models;

namespace Red.FTP.Interfaces;

public interface IFtpDataTransfer
{
    Task<IEnumerable<FtpFile>> GetFilesAsync(string remotePath, string ip, int port);
}
