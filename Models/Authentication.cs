namespace Red.FTP.Models;

public class Authentication
{
    public record BasicFtpCredentials(string User, string Password);
}

