using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;
using HarmanPCTools.Models;

namespace HarmanPCTools.Services;

public static class ThemeService
{
    private static readonly string SettingsDirectory =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HarmanPCTools");

    private static readonly string SettingsFile =
        Path.Combine(SettingsDirectory, "theme.json");

    public static IReadOnlyList<ThemeDefinition> Themes { get; } =
        new[]
        {
            new ThemeDefinition(
                "Harman Blue",
                "#080D14", "#0B121A", "#101923", "#151F2B", "#263544",
                "#F4F8FC", "#94A4B6", "#3A9EDB", "#163550",
                "#132332", "#162330", "#1B2836", "#45B1FF",
                "#69E4A4", "#F4CA6A", "#F18B8B"),

            new ThemeDefinition(
                "Carbon Steel",
                "#0B0E12", "#101419", "#171C22", "#20262E", "#343C46",
                "#F1F4F7", "#9AA4AE", "#7A9CAF", "#203341",
                "#171D24", "#1C2229", "#252C34", "#91BEDD",
                "#71D6A2", "#E3C46D", "#EE8F8F"),

            new ThemeDefinition(
                "Arctic Cyan",
                "#071114", "#0A171A", "#0F1D21", "#17272C", "#294148",
                "#EFF9FA", "#96ADB2", "#45B7BB", "#123B3E",
                "#13272A", "#162D30", "#1C363A", "#48D9DE",
                "#6FE1AB", "#E7CC6B", "#F08C8C"),

            new ThemeDefinition(
                "Violet Night",
                "#0D0912", "#130E19", "#1B1422", "#251B30", "#3E3150",
                "#F7F2FC", "#A89AAF", "#9876D3", "#322046",
                "#21182B", "#281E33", "#30263B", "#AE8CFF",
                "#79DFAB", "#EDD06D", "#F18E9A"),

            new ThemeDefinition(
                "Ember",
                "#120B08", "#180F0B", "#211610", "#2A1D16", "#4A3428",
                "#FBF5F0", "#B4A097", "#D98A52", "#4B2918",
                "#2A1B13", "#302016", "#39271D", "#FFAB76",
                "#70DCA0", "#EAC86B", "#F08E8E")
        };

    public static ThemeDefinition Current { get; private set; } = Themes[0];

    public static void LoadSavedTheme()
    {
        try
        {
            if (!File.Exists(SettingsFile))
            {
                ApplyTheme(Themes[0], save: false);
                return;
            }

            string json = File.ReadAllText(SettingsFile);
            string? name = JsonSerializer.Deserialize<string>(json);

            ApplyTheme(Find(name) ?? Themes[0], save: false);
        }
        catch
        {
            ApplyTheme(Themes[0], save: false);
        }
    }

    public static void ApplyTheme(ThemeDefinition theme, bool save = true)
    {
        Current = theme;

        SetBrush("BgBrush", theme.Background);
        SetBrush("SidebarBrush", theme.Sidebar);
        SetBrush("PanelBrush", theme.Panel);
        SetBrush("Panel2Brush", theme.Panel2);
        SetBrush("BorderBrush", theme.Border);
        SetBrush("TextBrush", theme.Text);
        SetBrush("MutedBrush", theme.Muted);
        SetBrush("AccentBrush", theme.Accent);
        SetBrush("AccentSoftBrush", theme.AccentSoft);
        SetBrush("NavHoverBrush", theme.NavHover);
        SetBrush("CardHoverBrush", theme.CardHover);
        SetBrush("SecondaryHoverBrush", theme.SecondaryHover);
        SetBrush("PrimaryHoverBrush", theme.PrimaryHover);
        SetBrush("SuccessBrush", theme.Success);
        SetBrush("WarningBrush", theme.Warning);
        SetBrush("DangerBrush", theme.Danger);
        SetBrush("WindowSurfaceBrush", theme.Panel2);

        if (save)
            SaveTheme(theme);
    }

    public static ThemeDefinition? Find(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        foreach (ThemeDefinition theme in Themes)
        {
            if (string.Equals(theme.Name, name, StringComparison.OrdinalIgnoreCase))
                return theme;
        }

        return null;
    }

    private static void SetBrush(string key, string hex)
    {
        try
        {
            Color color = (Color)ColorConverter.ConvertFromString(hex)!;
            // Replace the resource instead of mutating a possibly frozen WPF brush.
            Application.Current.Resources[key] = new SolidColorBrush(color);
        }
        catch
        {
            // Keep the previous resource on invalid theme data.
        }
    }

    private static void SaveTheme(ThemeDefinition theme)
    {
        try
        {
            Directory.CreateDirectory(SettingsDirectory);
            File.WriteAllText(SettingsFile, JsonSerializer.Serialize(theme.Name));
        }
        catch
        {
            // The current-session theme still works if saving is unavailable.
        }
    }
}
