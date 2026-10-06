using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using HarmanPCTools.Models;

namespace HarmanPCTools.Services;

public sealed class GameShortcutService
{
    private static readonly string SettingsDirectory =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HarmanPCTools");

    private static readonly string SettingsFile =
        Path.Combine(SettingsDirectory, "games.json");

    public List<GameShortcut> Games { get; private set; } = new();

    public GameShortcutService()
    {
        Load();
    }

    public void Add(string name, string path)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(path))
            return;

        if (!File.Exists(path))
            return;

        Games.RemoveAll(g => string.Equals(g.Path, path, StringComparison.OrdinalIgnoreCase));
        Games.Add(new GameShortcut(name.Trim(), path));
        Save();
    }

    public void Remove(GameShortcut game)
    {
        Games.RemoveAll(g => string.Equals(g.Path, game.Path, StringComparison.OrdinalIgnoreCase));
        Save();
    }

    public bool Launch(GameShortcut game, out string? error)
    {
        error = null;

        try
        {
            if (!File.Exists(game.Path))
            {
                error = "The game executable could not be found. Remove and add the game again.";
                return false;
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = game.Path,
                UseShellExecute = true
            });

            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }

    private void Load()
    {
        try
        {
            if (!File.Exists(SettingsFile))
                return;

            string json = File.ReadAllText(SettingsFile);
            Games = JsonSerializer.Deserialize<List<GameShortcut>>(json) ?? new List<GameShortcut>();
        }
        catch
        {
            Games = new List<GameShortcut>();
        }
    }

    private void Save()
    {
        try
        {
            Directory.CreateDirectory(SettingsDirectory);
            File.WriteAllText(SettingsFile,
                JsonSerializer.Serialize(Games, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch
        {
            // Favorites remain available for the current session.
        }
    }
}
