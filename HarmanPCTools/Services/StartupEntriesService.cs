using System.Collections.Generic;
using Microsoft.Win32;
using HarmanPCTools.Models;

namespace HarmanPCTools.Services;

public sealed class StartupEntriesService
{
    private const string RunPath = @"Software\Microsoft\Windows\CurrentVersion\Run";

    public List<StartupEntry> GetEntries()
    {
        List<StartupEntry> entries = new();
        Read(RegistryHive.CurrentUser, "Current User", entries);

        try
        {
            Read(RegistryHive.LocalMachine, "All Users", entries);
        }
        catch { }

        return entries;
    }

    private static void Read(RegistryHive hive, string source, List<StartupEntry> entries)
    {
        try
        {
            using RegistryKey? key =
                RegistryKey.OpenBaseKey(hive, RegistryView.Default).OpenSubKey(RunPath, false);

            if (key is null)
                return;

            foreach (string name in key.GetValueNames())
            {
                string command = key.GetValue(name)?.ToString() ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(command))
                    entries.Add(new StartupEntry(name, command, source));
            }
        }
        catch { }
    }
}
