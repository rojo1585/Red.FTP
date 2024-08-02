namespace Red.FTP.Models;

internal class Authentication
{
    public record BasicFtpCredentials(string User, string Password);
}

