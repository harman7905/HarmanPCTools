using System;
using System.IO;

namespace HarmanPCTools.Services;

public sealed class CleanupService
{
    public string UserTempPath => Path.GetTempPath();

    public long CalculateTempSize() => CalculateDirectorySize(UserTempPath);

    public int DeleteUserTempFiles(out long freedBytes)
    {
        freedBytes = 0;
        int deleted = 0;

        try
        {
            foreach (string file in Directory.EnumerateFiles(UserTempPath, "*", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    long size = new FileInfo(file).Length;
                    File.Delete(file);
                    freedBytes += size;
                    deleted++;
                }
                catch { }
            }

            foreach (string dir in Directory.EnumerateDirectories(UserTempPath))
            {
                try
                {
                    long size = CalculateDirectorySize(dir);
                    Directory.Delete(dir, true);
                    freedBytes += size;
                    deleted++;
                }
                catch { }
            }
        }
        catch { }

        return deleted;
    }

    public static string FormatBytes(long bytes)
    {
        const double kb = 1024;
        const double mb = kb * 1024;
        const double gb = mb * 1024;

        return bytes switch
        {
            < 1024 => $"{bytes} B",
            < 1024 * 1024 => $"{bytes / kb:0.0} KB",
            < 1024L * 1024L * 1024L => $"{bytes / mb:0.0} MB",
            _ => $"{bytes / gb:0.00} GB"
        };
    }

    private static long CalculateDirectorySize(string path)
    {
        long total = 0;

        try
        {
            foreach (string file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
            {
                try { total += new FileInfo(file).Length; }
                catch { }
            }
        }
        catch { }

        return total;
    }
}
