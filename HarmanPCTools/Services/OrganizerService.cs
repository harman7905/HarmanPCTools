using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HarmanPCTools.Services;

public sealed class OrganizerService
{
    private static readonly Dictionary<string, string> Categories = new(StringComparer.OrdinalIgnoreCase)
    {
        [".jpg"] = "Images", [".jpeg"] = "Images", [".png"] = "Images", [".gif"] = "Images",
        [".webp"] = "Images", [".bmp"] = "Images", [".svg"] = "Images",
        [".mp4"] = "Videos", [".mkv"] = "Videos", [".mov"] = "Videos", [".avi"] = "Videos", [".webm"] = "Videos",
        [".mp3"] = "Audio", [".wav"] = "Audio", [".m4a"] = "Audio", [".flac"] = "Audio",
        [".pdf"] = "Documents", [".doc"] = "Documents", [".docx"] = "Documents", [".txt"] = "Documents",
        [".xls"] = "Documents", [".xlsx"] = "Documents", [".ppt"] = "Documents", [".pptx"] = "Documents",
        [".zip"] = "Archives", [".rar"] = "Archives", [".7z"] = "Archives",
        [".exe"] = "Programs", [".msi"] = "Programs"
    };

    public string DownloadsPath =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

    public IReadOnlyDictionary<string, int> GetPreview()
    {
        Dictionary<string, int> result = new(StringComparer.OrdinalIgnoreCase);
        if (!Directory.Exists(DownloadsPath))
            return result;

        foreach (string file in Directory.EnumerateFiles(DownloadsPath, "*", SearchOption.TopDirectoryOnly))
        {
            string ext = Path.GetExtension(file);
            string category = Categories.TryGetValue(ext, out string? value)
                ? value
                : "Other";

            result[category] = result.GetValueOrDefault(category) + 1;
        }

        return result.OrderBy(kv => kv.Key).ToDictionary(kv => kv.Key, kv => kv.Value);
    }

    public int Organize()
    {
        if (!Directory.Exists(DownloadsPath))
            return 0;

        int moved = 0;

        foreach (string file in Directory.EnumerateFiles(DownloadsPath, "*", SearchOption.TopDirectoryOnly))
        {
            try
            {
                string ext = Path.GetExtension(file);
                string category = Categories.TryGetValue(ext, out string? value) ? value : "Other";
                string destinationDir = Path.Combine(DownloadsPath, category);
                Directory.CreateDirectory(destinationDir);

                string destination = Path.Combine(destinationDir, Path.GetFileName(file));
                destination = GetAvailableName(destination);

                File.Move(file, destination);
                moved++;
            }
            catch
            {
                // In-use or protected files are skipped.
            }
        }

        return moved;
    }

    private static string GetAvailableName(string desiredPath)
    {
        if (!File.Exists(desiredPath))
            return desiredPath;

        string dir = Path.GetDirectoryName(desiredPath)!;
        string name = Path.GetFileNameWithoutExtension(desiredPath);
        string ext = Path.GetExtension(desiredPath);

        int counter = 1;
        string candidate;
        do
        {
            candidate = Path.Combine(dir, $"{name} ({counter}){ext}");
            counter++;
        } while (File.Exists(candidate));

        return candidate;
    }
}
