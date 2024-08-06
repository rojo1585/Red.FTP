using Red.FTP.Client;
using Red.FTP.Handler;
using Red.FTP.Interfaces;
using Red.FTP.Modules;
using Red.FTP.Services;

namespace Red.FTP.Factory;

public static class FtpClientFactory
{
    public static FtpClient CreateTcpClient(bool useSsl = false, IFtpCommand? command = null, IFtpFileTransfer? dataTransfer = null, IConnection? passiveConnection = null, IFtpAuthenticator? authenticator = null)
    {
        command ??= new FtpCommands();
        dataTransfer ??= new FtpActions(command);
        passiveConnection ??= new FtpPassiveConnection(command);
        authenticator ??= new FtpAuthenticator(command);
        return new FtpClient(command, dataTransfer, passiveConnection, authenticator, useSsl);
    }
}
