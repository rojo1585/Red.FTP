using Red.FTP.Client;
using Red.FTP.Interfaces;
using Red.FTP.Modules;
using Red.FTP.Services;

namespace Red.FTP.Factory;

public static class FtpClientFactory
{
    public static FtpClient CreateTcpClient(IFtpCommand command, IFtpDataTransfer dataTransfer)
    {
        command ??= new FtpCommands();
        dataTransfer ??= new FtpActions(command);
        return new FtpClient(command, dataTransfer);
    }
}
