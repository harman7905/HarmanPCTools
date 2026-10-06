using System;
using Microsoft.Win32;

namespace HarmanPCTools.Services;

public static class StartupService
{
    private const string RunKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
    private const string AppName = "HarmanPCTools";

    public static bool IsEnabled()
    {
        try
        {
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(RunKey, false);
            return key?.GetValue(AppName) is string;
        }
        catch
        {
            return false;
        }
    }

    public static void SetEnabled(bool enabled)
    {
        try
        {
            using RegistryKey key = Registry.CurrentUser.CreateSubKey(RunKey);

            if (!enabled)
            {
                key.DeleteValue(AppName, false);
                return;
            }

            string executable = Environment.ProcessPath ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(executable))
                key.SetValue(AppName, $"\"{executable}\"");
        }
        catch { }
    }
}
