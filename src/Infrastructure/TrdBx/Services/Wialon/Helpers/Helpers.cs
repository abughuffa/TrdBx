namespace CleanArchitecture.Blazor.Infrastructure.Services.Wialon.Helpers;
/// <summary>
/// Wialon works only with unix formatted date
/// </summary>
public static class DateTimeToUnixFormatConverter
{
    public static int ToUnixTime(this DateTime dateTime)
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
        var span = dateTime.ToLocalTime() - epoch;
        return (int)Math.Round(span.TotalSeconds);
    }
}
public static class UnixFormatToDateTimeConverter
{
    public static DateTime ToDateTime(this int unixTime)
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return epoch.AddSeconds(unixTime).ToLocalTime();
    }
}
public class WialonExceptions
{
    private static readonly Dictionary<int, string> _exceptionsInfoByCode = new Dictionary<int, string>
        {
            {0, "Successful completion of the operation" },
            {1, "Invalid session" },
            {2, "Invalid service name" },
            {3, "Invalid result" },
            {4, "Invalid input" },
            {5, "Request execution error" },
            {6, "Unknown error" },
            {7, "Access Denied" },
            {8, "Incorrect password or username" },
            {9, "Authentication server is unavailable, please try your request again later" },
            {10, "Concurrent requests limit exceeded" },
            {11, "An error occurred while executing the password reset request" },
            {1001, "No messages for the selected interval" },
            {1002, "An item with this unique property already exists" },
            {1003, "Only one request is allowed at a time" },
            {1004, "Message limit exceeded" },
            {1005, "The execution time limit was exceeded" },
            {1011, "The session has expired or your IP has changed" },
            {2014, "The current user cannot be selected when creating an account" },
            {2015, "Deleting the sensor is prohibited due to being used in another sensor or additional object properties" }
        };


    public static string GetErrorMsg(int code)
    {
        var errorByCode = "Unknown error";
        try
        {
            return _exceptionsInfoByCode[code];
        }
        catch
        {
            return errorByCode;
        }
    }
}
