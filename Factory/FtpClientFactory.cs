using Red.FTP.Client;
using Red.FTP.Modules;
using Red.FTP.Services;

namespace Red.FTP.Factory;

public static class FtpClientFactory
{
    public static FtpClient CreateTcpClient()
    {
        var command = new FtpCommands();
        var transferData = new FtpDataTransfer(command);
        return new FtpClient(command, transferData);
    }
}
