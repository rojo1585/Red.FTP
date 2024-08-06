using static Red.FTP.Models.Literals;

namespace Red.FTP.Handler;

internal static class FtpStatusCodes
{
    private static readonly Dictionary<FtpStatusCode, string> _statusCodes = new()
{
    { FtpStatusCode.RestartMarkerReply, "Restart marker reply" },
    { FtpStatusCode.ServiceReadyInMinutes, "Service ready in nnn minutes" },
    { FtpStatusCode.DataConnectionAlreadyOpen, "Data connection already open; transfer starting" },
    { FtpStatusCode.FileStatusOkay, "File status okay; about to open data connection" },
    { FtpStatusCode.CommandOkay, "Command okay" },
    { FtpStatusCode.CommandNotImplemented, "Command not implemented, superfluous at this site" },
    { FtpStatusCode.SystemStatus, "System status, or system help reply" },
    { FtpStatusCode.DirectoryStatus, "Directory status" },
    { FtpStatusCode.FileStatus, "File status" },
    { FtpStatusCode.HelpMessage, "Help message" },
    { FtpStatusCode.NameSystemType, "NAME system type, where NAME is an official system name from the list in the Assigned Numbers document" },
    { FtpStatusCode.ServiceReadyForNewUser, "Service ready for new user" },
    { FtpStatusCode.ServiceClosingControlConnection, "Service closing control connection. Logged out if appropriate" },
    { FtpStatusCode.DataConnectionOpen, "Data connection open; no transfer in progress" },
    { FtpStatusCode.ClosingDataConnection, "Closing data connection. Requested file action successful (for example, file transfer or file abort)" },
    { FtpStatusCode.EnteringPassiveMode, "Entering Passive Mode (h1,h2,h3,h4,p1,p2)" },
    { FtpStatusCode.ExtendedPassiveModeEntered, "Extended passive mode entered" },
    { FtpStatusCode.UserLoggedIn, "User logged in, proceed" },
    { FtpStatusCode.UserLoggedInAuthorized, "User logged in, authorized by security data exchange" },
    { FtpStatusCode.SecurityDataExchangeComplete, "Security data exchange complete" },
    { FtpStatusCode.SecurityDataExchangeCompletedSuccessfully, "Security data exchange completed successfully" },
    { FtpStatusCode.RequestedFileActionOkay, "Requested file action okay, completed" },
    { FtpStatusCode.PathnameCreated, "Created" },
    { FtpStatusCode.UserNameOkayNeedPassword, "User name okay, need password" },
    { FtpStatusCode.NeedAccountForLogin, "Need account for login" },
    { FtpStatusCode.RequestedSecurityMechanismOkay, "Requested security mechanism ok" },
    { FtpStatusCode.SecurityDataAcceptable, "Security data is acceptable. More data is required to complete the security data exchange" },
    { FtpStatusCode.UsernameOkayNeedPassword, "Username okay, need password" },
    { FtpStatusCode.RequestedFileActionPendingFurtherInformation, "Requested file action pending further information" },
    { FtpStatusCode.ServiceNotAvailable, "Service not available, closing control connection. This may be a reply to any command if the service knows it must shut down" },
    { FtpStatusCode.CannotOpenDataConnection, "Cannot open data connection" },
    { FtpStatusCode.ConnectionClosedTransferAborted, "Connection closed; transfer aborted" },
    { FtpStatusCode.NeedUnavailableResource, "Need some unavailable resource to process security" },
    { FtpStatusCode.RequestedFileActionNotTaken, "Requested file action not taken. File unavailable (for example, file busy)" },
    { FtpStatusCode.RequestedActionAbortedLocalError, "Requested action aborted. Local error in processing" },
    { FtpStatusCode.RequestedActionNotTakenInsufficientStorage, "Requested action not taken. Insufficient storage space in system" },
    { FtpStatusCode.SyntaxErrorCommandUnrecognized, "Syntax error, command unrecognized. This may include errors such as command line too long" },
    { FtpStatusCode.SyntaxErrorInParametersOrArguments, "Syntax error in parameters or arguments" },
    { FtpStatusCode.BadSequenceOfCommands, "Bad sequence of commands" },
    { FtpStatusCode.CommandNotImplementedForParameter, "Command not implemented for that parameter" },
    { FtpStatusCode.DataConnectionCannotBeOpened, "Data connection cannot be opened with this PROT setting" },
    { FtpStatusCode.ServerDoesNotSupportRequestedProtocol, "Server does not support the requested network protocol" },
    { FtpStatusCode.NotLoggedIn, "Not logged in" },
    { FtpStatusCode.NeedAccountForStoringFiles, "Need account for storing files" },
    { FtpStatusCode.CommandProtectionLevelDenied, "Command protection level denied for policy reasons" },
    { FtpStatusCode.RequestDeniedForPolicyReasons, "Request denied for policy reasons" },
    { FtpStatusCode.FailedSecurityCheck, "Failed security check (hash, sequence, and so on)" },
    { FtpStatusCode.RequestedProtectionLevelNotSupported, "Requested PROT level not supported by mechanism" },
    { FtpStatusCode.CommandProtectionLevelNotSupported, "Command protection level not supported by security mechanism" },
    { FtpStatusCode.RequestedActionNotTakenFileUnavailable, "Requested action not taken. File unavailable" },
    { FtpStatusCode.RequestedActionAbortedPageTypeUnknown, "Requested action aborted: Page type unknown" },
    { FtpStatusCode.RequestedFileActionAbortedExceededStorage, "Requested file action aborted. Exceeded storage allocation (for current directory or dataset)" },
    { FtpStatusCode.RequestedActionNotTakenFileNameNotAllowed, "Requested action not taken. File name not allowed" },
    { FtpStatusCode.IntegrityProtectedReply, "Integrity protected reply" },
    { FtpStatusCode.ConfidentialityAndIntegrityProtectedReply, "Confidentiality and integrity protected reply" },
    { FtpStatusCode.ConfidentialityProtectedReply, "Confidentiality protected reply" }
    };


    private static readonly HashSet<FtpStatusCode> AuthenticatedStatusCodes =
    [
        FtpStatusCode.UserLoggedIn,
        FtpStatusCode.UserLoggedInAuthorized,
        FtpStatusCode.SecurityDataExchangeComplete,
        FtpStatusCode.SecurityDataExchangeCompletedSuccessfully
    ];

    private static HashSet<FtpStatusCode> FtpErrorCodes = new HashSet<FtpStatusCode>
        {
            // 4xx: Transient Negative Completion reply
            FtpStatusCode.ServiceNotAvailable,
            FtpStatusCode.CannotOpenDataConnection,
            FtpStatusCode.ConnectionClosedTransferAborted,
            FtpStatusCode.NeedUnavailableResource,
            FtpStatusCode.RequestedFileActionNotTaken,
            FtpStatusCode.RequestedActionAbortedLocalError,
            FtpStatusCode.RequestedActionNotTakenInsufficientStorage,

            // 5xx: Permanent Negative Completion reply
            FtpStatusCode.SyntaxErrorCommandUnrecognized,
            FtpStatusCode.SyntaxErrorInParametersOrArguments,
            FtpStatusCode.CommandNotImplemented,
            FtpStatusCode.BadSequenceOfCommands,
            FtpStatusCode.CommandNotImplementedForParameter,
            FtpStatusCode.DataConnectionCannotBeOpened,
            FtpStatusCode.ServerDoesNotSupportRequestedProtocol,
            FtpStatusCode.NotLoggedIn,
            FtpStatusCode.NeedAccountForStoringFiles,
            FtpStatusCode.CommandProtectionLevelDenied,
            FtpStatusCode.RequestDeniedForPolicyReasons,
            FtpStatusCode.FailedSecurityCheck,
            FtpStatusCode.RequestedProtectionLevelNotSupported,
            FtpStatusCode.CommandProtectionLevelNotSupported,
            FtpStatusCode.RequestedActionNotTakenFileUnavailable,
            FtpStatusCode.RequestedActionAbortedPageTypeUnknown,
            FtpStatusCode.RequestedFileActionAbortedExceededStorage,
            FtpStatusCode.RequestedActionNotTakenFileNameNotAllowed
        };
    public static string GetStatusCodeDescription(int statusCode)
    {
        if (_statusCodes.TryGetValue((FtpStatusCode)statusCode, out string? description))
        {
            return description;
        }
        return "Unknown status code";
    }
    public static bool IsAuthenticatedStatusCode(int statusCode) =>
        AuthenticatedStatusCodes.Contains((FtpStatusCode)statusCode);
    public static bool IsFtpError(int statusCode) =>
        FtpErrorCodes.Contains((FtpStatusCode)statusCode);


    public static (int statusCode, string description) GetStatusCodeAndMessage(string response)
    {
        if (response.Length < 3)
            throw new InvalidOperationException("Invalid FTP response format.");

        if (int.TryParse(response.AsSpan(0, 3), out int statusCode))
        {
            string message = GetStatusCodeDescription(statusCode);
            return (statusCode, message);
        }

        throw new InvalidOperationException("Invalid FTP response format.");
    }
}
