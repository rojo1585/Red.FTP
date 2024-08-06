namespace Red.FTP.Exceptions;

public class FtpConnectionException : Exception
{
    public FtpConnectionException() { }

    public FtpConnectionException(string message)
        : base(message) { }

    public FtpConnectionException(string message, Exception innerException)
        : base(message, innerException) { }
}
