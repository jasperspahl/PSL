using System.Text;

namespace PSL.Extensions;

public static class TimeSpanExtensions
{
    public static string PrettyPrint(this TimeSpan timeSpan)
    {
        var sb = new StringBuilder();
        if (timeSpan.Days > 0)
        {
            sb.Append($"{timeSpan.Days} day{(timeSpan.Days > 1 ? "s": "")} ");
        }
        if (timeSpan.Hours > 0)
        {
            sb.Append($"{timeSpan.Hours} hour{(timeSpan.Hours > 1 ? "s": "")} ");
        }
        if (timeSpan.Minutes > 0)
        {
            sb.Append($"{timeSpan.Minutes} minute{(timeSpan.Minutes > 1 ? "s": "")} ");
        }
        if (timeSpan.Seconds > 0)
        {
            sb.Append($"{timeSpan.Seconds} second{(timeSpan.Seconds > 1 ? "s": "")} ");
        }
        return sb.ToString();
    }
}