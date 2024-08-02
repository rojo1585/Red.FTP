using Red.FTP.Interfaces;
using Red.FTP.Models;
using System.Net.Sockets;
using System.Text;

namespace Red.FTP.Services;

internal class FtpActions(IFtpCommand _commandSender) : IFtpDataTransfer
{
    private TcpClient? _dataClient;
    private NetworkStream? _dataStream;

    public async Task<IEnumerable<FtpFile>> GetFilesAsync(string remotePath)
    {
        try
        {
            _commandSender.SendCommand($"LIST {remotePath}");
            var dataResponse = await ReadDataResponseAsync();
            await _commandSender.ReadResponseAsync();
            return dataResponse;
        }
        finally
        {
            CloseConnections();
        }
    }

    public async Task CreateDataClient(string ip, int port)
    {
        _dataClient = new();
        await _dataClient.ConnectAsync(ip, port);
        _dataStream = _dataClient.GetStream();
    }
    public async Task DownloadFileAsync(string localPath, string remotePath)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(_dataStream);

            _commandSender.SendCommand($"RETR {remotePath}");

            var (statusCode, description) = Handler.FtpStatusCodes.GetStatusCodeAndMessage(await _commandSender.ReadResponseAsync());
            if (statusCode == 550)
                throw new FileNotFoundException(description);

            using (FileStream fileStream = new(localPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = await _dataStream.ReadAsync(buffer)) > 0)
                {
                    await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));
                }
            }
        }
        finally
        {
            CloseConnections();
        }
    }

    private async Task<IEnumerable<FtpFile>> ReadDataResponseAsync()
    {
        ArgumentNullException.ThrowIfNull(_dataStream);

        List<FtpFile> dataResponse = [];
        byte[] buffer = new byte[1024];
        int bytesRead;

        try
        {
            while ((bytesRead = await _dataStream.ReadAsync(buffer)) > 0)
            {
                Helpers.FtpResponseParser.FtpStringToFtpFileList(ref dataResponse, Encoding.ASCII.GetString(buffer, 0, bytesRead));
            }
        }
        finally
        {
            _dataStream?.Close();
            _dataClient?.Close();
        }
        return dataResponse;
    }

    private void CloseConnections()
    {
        _dataClient?.Close();
        _dataStream?.Close();
    }
}
