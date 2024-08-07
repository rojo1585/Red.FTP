# Red.FTP

Red.FTP is a .NET library for easy and secure FTP connections, including support for FTPS (FTP over SSL/TLS).

## Features

- Standard FTP connections
- Support for FTPS (FTP over SSL/TLS)
- Task cancellation handling with `CancellationToken`
- Sending and receiving FTP commands

static async Task Main(string[] args)
    {
        var ftpClient = new FtpClient("ftp.example.com", 21);
        ftpClient.SetCredentials("username", "password");

        // Connect to the FTP server
        await ftpClient.ConnectAsync();

        // Authenticate
        var cancellationToken = new CancellationTokenSource().Token;
        var response = await ftpClient.AuthenticateAsync(cancellationToken);

        Console.WriteLine($"Authentication Response: {response.Description}");
    }