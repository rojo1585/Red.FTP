namespace Red.FTP.Handler;

internal static class FtpStatusCodes
{
    private static readonly Dictionary<int, string> _statusCodes = new()
    {
        { 110, "Restart marker reply" },
        { 120, "Service ready in nnn minutes" },
        { 125, "Data connection already open; transfer starting" },
        { 150, "File status okay; about to open data connection" },
        { 200, "Command okay" },
        { 202, "Command not implemented, superfluous at this site" },
        { 211, "System status, or system help reply" },
        { 212, "Directory status" },
        { 213, "File status" },
        { 214, "Help message" },
        { 215, "NAME system type, where NAME is an official system name from the list in the Assigned Numbers document" },
        { 220, "Service ready for new user" },
        { 221, "Service closing control connection. Logged out if appropriate" },
        { 225, "Data connection open; no transfer in progress" },
        { 226, "Closing data connection. Requested file action successful (for example, file transfer or file abort)" },
        { 227, "Entering Passive Mode (h1,h2,h3,h4,p1,p2)" },
        { 229, "Extended passive mode entered" },
        { 230, "User logged in, proceed" },
        { 232, "User logged in, authorized by security data exchange" },
        { 234, "Security data exchange complete" },
        { 235, "Security data exchange completed successfully" },
        { 250, "Requested file action okay, completed" },
        { 257, "\"PATHNAME\" created" },
        { 331, "User name okay, need password" },
        { 332, "Need account for login" },
        { 334, "Requested security mechanism ok" },
        { 335, "Security data is acceptable. More data is required to complete the security data exchange" },
        { 336, "Username okay, need password" },
        { 350, "Requested file action pending further information" },
        { 421, "Service not available, closing control connection. This may be a reply to any command if the service knows it must shut down" },
        { 425, "Cannot open data connection" },
        { 426, "Connection closed; transfer aborted" },
        { 431, "Need some unavailable resource to process security" },
        { 450, "Requested file action not taken. File unavailable (for example, file busy)" },
        { 451, "Requested action aborted. Local error in processing" },
        { 452, "Requested action not taken. Insufficient storage space in system" },
        { 500, "Syntax error, command unrecognized. This may include errors such as command line too long" },
        { 501, "Syntax error in parameters or arguments" },
        { 502, "Command not implemented" },
        { 503, "Bad sequence of commands" },
        { 504, "Command not implemented for that parameter" },
        { 521, "Data connection cannot be opened with this PROT setting" },
        { 522, "Server does not support the requested network protocol" },
        { 530, "Not logged in" },
        { 532, "Need account for storing files" },
        { 533, "Command protection level denied for policy reasons" },
        { 534, "Request denied for policy reasons" },
        { 535, "Failed security check (hash, sequence, and so on)" },
        { 536, "Requested PROT level not supported by mechanism" },
        { 537, "Command protection level not supported by security mechanism" },
        { 550, "Requested action not taken. File unavailable" },
        { 551, "Requested action aborted: Page type unknown" },
        { 552, "Requested file action aborted. Exceeded storage allocation (for current directory or dataset)" },
        { 553, "Requested action not taken. File name not allowed" },
        { 631, "Integrity protected reply" },
        { 632, "Confidentiality and integrity protected reply" },
        { 633, "Confidentiality protected reply" }
    };

    private static readonly HashSet<int> AuthenticatedStatusCodes = new HashSet<int>
    {
        230, // User logged in, proceed.
        232, // User logged in, authorized by security data exchange.
        234, // Security data exchange complete.
        235  // Security data exchange completed successfully.
    };
    public static string GetStatusCodeDescription(int statusCode)
    {
        if (_statusCodes.TryGetValue(statusCode, out var description))
        {
            return description;
        }
        return "Unknown status code";
    }
    public static bool IsAuthenticatedStatusCode(int statusCode)
    {
        return AuthenticatedStatusCodes.Contains(statusCode);
    }

}
