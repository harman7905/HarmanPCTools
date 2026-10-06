using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Microsoft.Win32;

namespace HarmanPCTools.Services;

public static class ObsLaunchService
{
    private static readonly string SettingsDirectory =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HarmanPCTools");

    private static readonly string SettingsFile =
        Path.Combine(SettingsDirectory, "obs.json");

    public static bool TryLaunch(out string? error)
    {
        error = null;

        string? path = GetSavedPath() ?? FindCommonPath();

        if (string.IsNullOrWhiteSpace(path))
        {
            error = "OBS Studio was not found. Use Configure OBS to select obs64.exe.";
            return false;
        }

        return TryStart(path, out error);
    }

    public static bool TryLaunchWithPrompt(out string? error)
    {
        if (TryLaunch(out error))
            return true;

        if (!TryConfigure(out error))
            return false;

        string? path = GetSavedPath();
        if (path is null)
        {
            error = "OBS Studio path was not saved.";
            return false;
        }

        return TryStart(path, out error);
    }

    public static bool TryConfigure(out string? error)
    {
        error = null;

        OpenFileDialog dialog = new()
        {
            Title = "Locate OBS Studio (obs64.exe)",
            Filter = "OBS Studio executable (obs64.exe)|obs64.exe|Applications (*.exe)|*.exe",
            Multiselect = false
        };

        if (dialog.ShowDialog() != true)
        {
            error = "OBS Studio selection was cancelled.";
            return false;
        }

        if (!string.Equals(Path.GetFileName(dialog.FileName), "obs64.exe", StringComparison.OrdinalIgnoreCase))
        {
            error = "Please select OBS Studio's obs64.exe.";
            return false;
        }

        SavePath(dialog.FileName);
        return true;
    }

    public static string? GetConfiguredPath() => GetSavedPath() ?? FindCommonPath();

    private static bool TryStart(string path, out string? error)
    {
        error = null;

        try
        {
            string workingDirectory = Path.GetDirectoryName(path)!;

            Process.Start(new ProcessStartInfo
            {
                FileName = path,
                WorkingDirectory = workingDirectory,
                UseShellExecute = true
            });

            return true;
        }
        catch (Exception ex)
        {
            error = $"Could not launch OBS Studio.\n\n{ex.Message}";
            return false;
        }
    }

    private static string? GetSavedPath()
    {
        try
        {
            if (!File.Exists(SettingsFile))
                return null;

            string? path = JsonSerializer.Deserialize<string>(File.ReadAllText(SettingsFile));
            return !string.IsNullOrWhiteSpace(path) && File.Exists(path) ? path : null;
        }
        catch
        {
            return null;
        }
    }

    private static void SavePath(string path)
    {
        Directory.CreateDirectory(SettingsDirectory);
        File.WriteAllText(SettingsFile, JsonSerializer.Serialize(path));
    }

    private static string? FindCommonPath()
    {
        string[] candidates =
        {
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "obs-studio", "bin", "64bit", "obs64.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                "obs-studio", "bin", "64bit", "obs64.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Programs", "obs-studio", "bin", "64bit", "obs64.exe")
        };

        foreach (string candidate in candidates)
        {
            if (File.Exists(candidate))
            {
                SavePath(candidate);
                return candidate;
            }
        }

        return null;
    }
}
