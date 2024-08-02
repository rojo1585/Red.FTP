using Red.FTP.Models;

namespace Red.FTP.Interfaces;

public interface IFtpDataTransfer
{
    Task CreateDataClient(string ip, int port);
    Task<IEnumerable<FtpFile>> GetFilesAsync(string remotePath);
    Task DownloadFileAsync(string localPath, string remotePath);

}
