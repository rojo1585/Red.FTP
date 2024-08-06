using Red.FTP.Exceptions;
using Red.FTP.Handler;
using Red.FTP.Interfaces;
using Red.FTP.Models;
using System.Net.Sockets;
using System.Text;

namespace Red.FTP.Services;

internal class FtpActions(IFtpCommand _commandSender) : IFtpFileTransfer
{
    public async Task<IEnumerable<FtpFile>> GetFilesAsync(NetworkStream stream, string remotePath, CancellationToken cancellationToken = default)
    {
        _commandSender.SendCommand($"LIST {remotePath}");
        var dataResponse = await ReadDataResponseAsync(stream, cancellationToken);
        await _commandSender.ReadResponseAsync(cancellationToken);
        return dataResponse;
    }

    public async Task DownloadFileAsync(NetworkStream stream, string localPath, string remoteFile, CancellationToken cancellationToken = default)
    {
        _commandSender.SendCommand($"RETR {remoteFile}");

        var (statusCode, description) = Handler.FtpStatusCodes.GetStatusCodeAndMessage(await _commandSender.ReadResponseAsync(cancellationToken));
        if (statusCode == (int)Literals.FtpStatusCode.RequestedActionNotTakenFileUnavailable)
            throw new Exceptions.FileNotFoundException(description);

        using FileStream fileStream = new(localPath, FileMode.Create, FileAccess.Write, FileShare.None);
        byte[] buffer = new byte[1024];
        int bytesRead;

        while ((bytesRead = await stream.ReadAsync(buffer, cancellationToken)) > 0)
        {
            await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    private static async Task<IEnumerable<FtpFile>> ReadDataResponseAsync(NetworkStream stream, CancellationToken cancellationToken = default)
    {
        List<FtpFile> dataResponse = [];
        byte[] buffer = new byte[1024];
        int bytesRead;

        while ((bytesRead = await stream.ReadAsync(buffer, cancellationToken)) > 0)
        {
            Helpers.FtpResponseParser.FtpStringToFtpFileList(ref dataResponse, Encoding.ASCII.GetString(buffer, 0, bytesRead));
            cancellationToken.ThrowIfCancellationRequested();
        }

        return dataResponse;
    }
    public async Task<int> UploadFileAsync(NetworkStream stream, string localPath, string remotePath, CancellationToken cancellationToken = default)
    {
        using var fileStream = new FileStream(localPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        _commandSender.SendCommand($"STOR {remotePath}");

        var (statusCode, description) = Handler.FtpStatusCodes.GetStatusCodeAndMessage(await _commandSender.ReadResponseAsync(cancellationToken));
        if (FtpStatusCodes.IsFtpError(statusCode))
            throw new FtpException($"Erro: {description}");

        byte[] buffer = new byte[1024];
        int bytesRead;
        while ((bytesRead = await fileStream.ReadAsync(buffer, cancellationToken)) > 0)
        {
            await stream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
        }
        return bytesRead;
    }

}
