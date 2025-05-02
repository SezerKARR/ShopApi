using System.Runtime.CompilerServices;

namespace ShopApi.Helpers;

public static class LoggerExtensions
{
    public static void LogWithLocation<T>(
        this ILogger<T> logger,
        string message,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0,
        [CallerMemberName] string memberName = "")
    {
        var fileName = Path.GetFileName(filePath);
        var finalMessage = $"[{fileName}:{lineNumber} - {memberName}] {message}";
        logger.LogInformation(finalMessage); 
    }

}