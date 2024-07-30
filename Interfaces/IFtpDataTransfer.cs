using Red.FTP.Models;

namespace Red.FTP.Interfaces;

internal interface IFtpDataTransfer
{
    Task<IEnumerable<FtpFile>> GetFilesAsync(string remotePath);
}
