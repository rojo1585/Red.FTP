namespace Red.FTP.Exemptions;

public class FtpAuthenticationException : Exception
{
    public FtpAuthenticationException() { }

    public FtpAuthenticationException(string message)
        : base(message) { }

    public FtpAuthenticationException(string message, Exception innerException)
        : base(message, innerException) { }
}


