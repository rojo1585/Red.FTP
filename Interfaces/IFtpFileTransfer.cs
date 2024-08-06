using Red.FTP.Models;
using System.Net.Sockets;

namespace Red.FTP.Interfaces;

public interface IFtpFileTransfer
{
    Task<IEnumerable<FtpFile>> GetFilesAsync(NetworkStream stream, string remotePath, CancellationToken cancellationToken = default);
    Task DownloadFileAsync(NetworkStream stream, string localPath, string remoteFile, CancellationToken cancellationToken = default);
}
