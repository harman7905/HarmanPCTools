using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using HarmanPCTools.Services;

namespace HarmanPCTools.Pages;

public partial class SettingsPage : UserControl
{
    private const string OfficialRepositoryUrl = "https://github.com/harman7905/HarmanPCTools";
    private const string LatestReleaseUrl = "https://github.com/harman7905/HarmanPCTools/releases/latest";
    private const string ReleasesUrl = "https://github.com/harman7905/HarmanPCTools/releases";

    public SettingsPage()
    {
        InitializeComponent();
        RefreshView();
    }

    private void RefreshView()
    {
        var settings = AppSettingsService.Current;
        StartWithWindowsCheck.IsChecked = settings.StartWithWindows;
        OpenLastPageCheck.IsChecked = settings.OpenLastPageOnStartup;
        DataFolderText.Text = AppSettingsService.DirectoryPath;
        LastPageText.Text = settings.LastOpenedPage;
        StartupStatusText.Text = settings.StartWithWindows ? "Enabled" : "Disabled";
    }

    private void StartWithWindows_Click(object sender, RoutedEventArgs e)
    {
        AppSettingsService.SetStartWithWindows(StartWithWindowsCheck.IsChecked == true);
        RefreshView();
    }

    private void OpenLastPage_Click(object sender, RoutedEventArgs e)
    {
        AppSettingsService.SetOpenLastPageOnStartup(OpenLastPageCheck.IsChecked == true);
        RefreshView();
    }

    private void RepositoryLink_Click(object sender, RoutedEventArgs e)
    {
        OpenUrl(OfficialRepositoryUrl, "The GitHub repository could not be opened.");
    }

    private void LatestReleaseLink_Click(object sender, RoutedEventArgs e)
    {
        OpenUrl(LatestReleaseUrl, "The latest release page could not be opened.");
    }

    private void OpenRepository_Click(object sender, RoutedEventArgs e)
    {
        OpenUrl(OfficialRepositoryUrl, "The GitHub repository could not be opened.");
    }

    private void OpenLatestRelease_Click(object sender, RoutedEventArgs e)
    {
        OpenUrl(LatestReleaseUrl, "The latest release page could not be opened.");
    }

    private void CheckForUpdates_Click(object sender, RoutedEventArgs e)
    {
        OpenUrl(ReleasesUrl, "The GitHub releases page could not be opened.");
    }

    private static void OpenUrl(string url, string errorMessage)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch
        {
            MessageBox.Show(errorMessage, "Harman PC Toolkit", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void OpenDataFolder_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = $"\"{AppSettingsService.DirectoryPath}\"",
                UseShellExecute = true
            });
        }
        catch
        {
            MessageBox.Show("The application data folder could not be opened.", "Harman PC Toolkit", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void ResetSettings_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show(
            "Reset Harman PC Toolkit settings to their defaults? This will also disable Start with Windows.",
            "Reset Settings",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes)
            return;

        AppSettingsService.ResetToDefaults();
        RefreshView();
        MessageBox.Show("Settings have been restored to their defaults.", "Harman PC Toolkit", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
