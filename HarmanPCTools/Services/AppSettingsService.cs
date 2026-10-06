using System;
using System.IO;
using System.Text.Json;
using HarmanPCTools.Models;

namespace HarmanPCTools.Services;

public static class AppSettingsService
{
    private static readonly string DirectoryPath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HarmanPCTools");

    private static readonly string FilePath =
        Path.Combine(DirectoryPath, "settings.json");

    public static AppSettings Current { get; private set; } = new();

    public static void Load()
    {
        try
        {
            if (File.Exists(FilePath))
                Current = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(FilePath)) ?? new();
        }
        catch
        {
            Current = new();
        }
    }

    public static void Save()
    {
        try
        {
            Directory.CreateDirectory(DirectoryPath);
            File.WriteAllText(FilePath,
                JsonSerializer.Serialize(Current, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch { }
    }

    public static void SetStartWithWindows(bool enabled)
    {
        Current.StartWithWindows = enabled;
        StartupService.SetEnabled(enabled);
        Save();
    }

    public static void SetGitHubRepositoryUrl(string url)
    {
        if (!string.IsNullOrWhiteSpace(url))
            Current.GitHubRepositoryUrl = url.Trim();

        Save();
    }
}
