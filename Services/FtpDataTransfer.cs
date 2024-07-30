using Red.FTP.Interfaces;
using Red.FTP.Models;
using System.Net.Sockets;
using System.Text;

namespace Red.FTP.Services;

internal class FtpDataTransfer : IFtpDataTransfer
{
    private TcpClient? _dataClient;
    private NetworkStream? _dataStream;
    private readonly IFtpCommandSender _commandSender;

    public FtpDataTransfer(IFtpCommandSender commandSender)
    {
        _commandSender = commandSender;
    }

    public async Task<IEnumerable<FtpFile>> GetFilesAsync(string remotePath)
    {
        _commandSender.SendCommand("PASV");
        string response = await _commandSender.ReadResponseAsync();
        var (ip, port) = ParsePasvResponse(response);

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
        List<FtpFile> dataResponse = new();
        byte[] buffer = new byte[1024];
        int bytesRead;

        try
        {
            while ((bytesRead = await _dataStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                BuildFtpFile(ref dataResponse, Encoding.ASCII.GetString(buffer, 0, bytesRead));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading data response: {ex.Message}");
            throw;
        }
        finally
        {
            _dataStream?.Close();
            _dataClient?.Close();
        }

        return dataResponse;
    }

    private static void BuildFtpFile(ref List<FtpFile> ftpFiles, string line)
    {
        var separator = new string[] { "\r\n" };
        var parts = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        foreach (var item in parts)
        {
            var cleanLine = item.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (cleanLine.Length < 4)
            {
                ftpFiles.Add(new FtpFile { Name = cleanLine.ToString() });
                continue;
            }
            var date = cleanLine[0];
            var time = cleanLine[1];
            var size = cleanLine[2];
            var name = string.Join(" ", cleanLine.Skip(3));

            ftpFiles.Add(new FtpFile
            {
                Date = date,
                Time = time,
                Size = size,
                Name = name
            });
        }
    }

    private static (string, int) ParsePasvResponse(string response)
    {
        var start = response.IndexOf('(') + 1;
        var end = response.IndexOf(')');
        var pasvData = response[start..end];
        var parts = pasvData.Split(',');
        var ip = $"{parts[0]}.{parts[1]}.{parts[2]}.{parts[3]}";
        var port = (int.Parse(parts[4]) * 256) + int.Parse(parts[5]);
        return (ip, port);
    }

}
