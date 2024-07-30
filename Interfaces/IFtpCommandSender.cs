namespace Red.FTP.Interfaces;

internal interface IFtpCommandSender
{
    void SendCommand(string command);
    Task<string> ReadResponseAsync();
}
