using Red.FTP.Interfaces;
using Red.FTP.Models;
using System.Net.Sockets;
using System.Text;

namespace Red.FTP.Services;

internal class FtpDataTransfer(IFtpCommand _commandSender) : IFtpDataTransfer
{
    private TcpClient? _dataClient;
    private NetworkStream? _dataStream;

    public async Task<IEnumerable<FtpFile>> GetFilesAsync(string remotePath, string ip, int port)
    {
        _dataClient = new TcpClient();
        await _dataClient.ConnectAsync(ip, port);
        _dataStream = _dataClient.GetStream();

        _commandSender.SendCommand($"LIST {remotePath}");
        var dataResponse = await ReadDataResponseAsync();
        await _commandSender.ReadResponseAsync();

        return dataResponse;
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



}
