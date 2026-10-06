using System;
using System.Diagnostics;
using System.IO;

namespace HarmanPCTools.Services;

public static class ApplicationLauncherService
{
    public static bool TryLaunch(string target, string label, out string? error)
    {
        error = null;

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = target,
                UseShellExecute = true
            });
            return true;
        }
        catch (Exception ex)
        {
            error = $"Could not open {label}: {ex.Message}";
            return false;
        }
    }

    public static bool TryLaunchObs(out string? error)
    {
        string[] candidates =
        {
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "obs-studio", "bin", "64bit", "obs64.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "obs-studio", "bin", "64bit", "obs64.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Programs", "obs-studio", "bin", "64bit", "obs64.exe")
        };

        foreach (string path in candidates)
        {
            if (File.Exists(path))
                return TryLaunch(path, "OBS Studio", out error);
        }

        error = "OBS Studio was not found in the common installation locations.";
        return false;
    }

    public static bool TryLaunchNvidiaControlPanel(out string? error)
    {
        string path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            "NVIDIA Corporation",
            "Control Panel Client",
            "nvcplui.exe");

        if (File.Exists(path))
            return TryLaunch(path, "NVIDIA Control Panel", out error);

        error = "NVIDIA Control Panel was not found in its common installation location.";
        return false;
    }
}
