namespace Red.FTP.Models;

public static class Literals
{
    public enum FtpStatusCode
    {
        // 1xx: Positive Preliminary reply
        RestartMarkerReply = 110,
        ServiceReadyInMinutes = 120,
        DataConnectionAlreadyOpen = 125,
        FileStatusOkay = 150,

        // 2xx: Positive Completion reply
        CommandOkay = 200,
        CommandNotImplemented = 202,
        SystemStatus = 211,
        DirectoryStatus = 212,
        FileStatus = 213,
        HelpMessage = 214,
        NameSystemType = 215,
        ServiceReadyForNewUser = 220,
        ServiceClosingControlConnection = 221,
        DataConnectionOpen = 225,
        ClosingDataConnection = 226,
        EnteringPassiveMode = 227,
        ExtendedPassiveModeEntered = 229,
        UserLoggedIn = 230,
        UserLoggedInAuthorized = 232,
        SecurityDataExchangeComplete = 234,
        SecurityDataExchangeCompletedSuccessfully = 235,
        RequestedFileActionOkay = 250,
        PathnameCreated = 257,

        // 3xx: Positive Intermediate reply
        UserNameOkayNeedPassword = 331,
        NeedAccountForLogin = 332,
        RequestedSecurityMechanismOkay = 334,
        SecurityDataAcceptable = 335,
        UsernameOkayNeedPassword = 336,
        RequestedFileActionPendingFurtherInformation = 350,

        // 4xx: Transient Negative Completion reply
        ServiceNotAvailable = 421,
        CannotOpenDataConnection = 425,
        ConnectionClosedTransferAborted = 426,
        NeedUnavailableResource = 431,
        RequestedFileActionNotTaken = 450,
        RequestedActionAbortedLocalError = 451,
        RequestedActionNotTakenInsufficientStorage = 452,

        // 5xx: Permanent Negative Completion reply
        SyntaxErrorCommandUnrecognized = 500,
        SyntaxErrorInParametersOrArguments = 501,
        CommandNotImplementeds = 502,
        BadSequenceOfCommands = 503,
        CommandNotImplementedForParameter = 504,
        DataConnectionCannotBeOpened = 521,
        ServerDoesNotSupportRequestedProtocol = 522,
        NotLoggedIn = 530,
        NeedAccountForStoringFiles = 532,
        CommandProtectionLevelDenied = 533,
        RequestDeniedForPolicyReasons = 534,
        FailedSecurityCheck = 535,
        RequestedProtectionLevelNotSupported = 536,
        CommandProtectionLevelNotSupported = 537,
        RequestedActionNotTakenFileUnavailable = 550,
        RequestedActionAbortedPageTypeUnknown = 551,
        RequestedFileActionAbortedExceededStorage = 552,
        RequestedActionNotTakenFileNameNotAllowed = 553,

        // 6xx: Protected reply
        IntegrityProtectedReply = 631,
        ConfidentialityAndIntegrityProtectedReply = 632,
        ConfidentialityProtectedReply = 633
    }

}
