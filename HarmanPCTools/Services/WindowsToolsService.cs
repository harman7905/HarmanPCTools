using System;
using System.Diagnostics;
using System.Windows;

namespace HarmanPCTools.Services;

public static class WindowsToolsService
{
    public static bool TryOpen(string fileName, string? arguments, string label, out string? error)
    {
        error = null;

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments ?? string.Empty,
                UseShellExecute = true
            });

            return true;
        }
        catch (Exception ex)
        {
            error = $"Could not open {label}.\n\n{ex.Message}";
            return false;
        }
    }

    public static void Open(string fileName, string? arguments = null, string? label = null)
    {
        if (!TryOpen(fileName, arguments, label ?? fileName, out string? error) &&
            !string.IsNullOrWhiteSpace(error))
        {
            MessageBox.Show(error, "Harman PC Tools");
        }
    }

    public static void OpenSettings(string uri, string label)
    {
        if (!TryOpen(uri, null, label, out string? error) &&
            !string.IsNullOrWhiteSpace(error))
        {
            MessageBox.Show(error, "Harman PC Tools");
        }
    }
}
