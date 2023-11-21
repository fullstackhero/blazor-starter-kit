using MudBlazor;

namespace CleanBlazor.Client.Extensions;

public static class SnackBarExtensions
{
    public static Snackbar Success(this ISnackbar snackBar, string message) => snackBar.Add(message, Severity.Success);

    public static void Success(this ISnackbar snackbar, IEnumerable<string> messages)
    {
        foreach (var message in messages)
        {
            snackbar.Success(message);
        }
    }

    public static Snackbar Error(this ISnackbar snackBar, string message) => snackBar.Add(message, Severity.Error);

    public static void Error(this ISnackbar snackbar, IEnumerable<string> messages)
    {
        foreach (var message in messages)
        {
            snackbar.Error(message);
        }
    }

    public static Snackbar Info(this ISnackbar snackBar, string message) => snackBar.Add(message, Severity.Info);

    public static void Info(this ISnackbar snackbar, IEnumerable<string> messages)
    {
        foreach (var message in messages)
        {
            snackbar.Info(message);
        }
    }
}
