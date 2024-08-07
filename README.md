# Red.FTP

Red.FTP is a .NET library for easy and secure FTP connections, including support for FTPS (FTP over SSL/TLS).

## Features

- Standard FTP connections
- Support for FTPS (FTP over SSL/TLS)
- Task cancellation handling with `CancellationToken`
- Sending and receiving FTP commands

## Example

### Login
<sup>
            //Create Client whit default config
            var client = FtpClientFactory.CreateTcpClient();
            var conn = await client.CreateConnectionAsync("Host");
            
            //Authentication
            client.SetCredentials("FtpUSer", "FtpPassword");
            var auth = await client.AuthAsync();
</sup>

### Get Files List
<sup>

            var cancelationToken = new CancellationTokenSource();
            cancelationToken.CancelAfter(TimeSpan.FromSeconds(100));
            string remotePath = "/"

            var file = await client.GetFilesAsync(remotePath, cancelationToken.Token);
</sup>

### Download file
<sup>

            var cancelationToken = new CancellationTokenSource();
            cancelationToken.CancelAfter(TimeSpan.FromSeconds(100));
            string remotePath = "/"
            string localPath = "C:"

            var file = await client.GetFilesAsync(localPath, remotePath,cancelationToken.Token);
</sup>

###
### Upload file
<sup>

            var cancelationToken = new CancellationTokenSource();
            cancelationToken.CancelAfter(TimeSpan.FromSeconds(100));
            string localFile = "c:/test.zip"
            string remotePath = "/"

            var file = await client.GetFilesAsync(localPath, remotePath, cancelationToken.Token);
</sup>