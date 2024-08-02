using Red.FTP.Models;

namespace Red.FTP.Helpers;

internal static class FtpResponseParser
{
    public static (string, int) ParsePasiveResponse(string response)
    {
        var start = response.IndexOf('(') + 1;
        var end = response.IndexOf(')');
        var pasiveData = response.Substring(start, end - start);
        var parts = pasiveData.Split(',');
        var ip = $"{parts[0]}.{parts[1]}.{parts[2]}.{parts[3]}";
        var port = (int.Parse(parts[4]) * 256) + int.Parse(parts[5]);
        return (ip, port);
    }
    public static void FtpStringToFtpFileList(ref List<FtpFile> ftpFiles, string line)
    {
        var separator = new string[] { "\r\n" };
        var parts = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        foreach (var item in parts)
        {
            var cleanLine = item.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (cleanLine.Length < 4)
                continue;

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
}
