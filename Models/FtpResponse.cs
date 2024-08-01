namespace Red.FTP.Models;

public class FtpResponse(int statusCode, string description, dynamic? result = null)
{
    public int StatusCode { get; set; } = statusCode;
    public string? Description { get; set; } = description;
    public dynamic? Result { get; set; } = result;

}
