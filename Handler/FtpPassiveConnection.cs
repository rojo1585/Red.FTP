using Red.FTP.Helpers;
using Red.FTP.Interfaces;

namespace Red.FTP.Handler;

internal class FtpPassiveConnection(IFtpCommand commands, IFtpDataTransfer ftpDataTransfer)
{
    public async Task CreateConnectionAsync()
    {
        commands.SendCommand("PASV");
        string response = await commands.ReadResponseAsync();
        var (ip, port) = FtpResponseParser.ParsePasiveResponse(response);
        await ftpDataTransfer.CreateDataClient(ip, port);
    }
}
