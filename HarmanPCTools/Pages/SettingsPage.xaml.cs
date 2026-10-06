using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using HarmanPCTools.Services;

namespace HarmanPCTools.Pages;

public partial class SettingsPage : UserControl
{
    public SettingsPage()
    {
        InitializeComponent();
        StartWithWindowsCheck.IsChecked = AppSettingsService.Current.StartWithWindows;
        RepositoryTextBox.Text = AppSettingsService.Current.GitHubRepositoryUrl;
    }

    private void StartWithWindows_Click(object sender, RoutedEventArgs e) =>
        AppSettingsService.SetStartWithWindows(StartWithWindowsCheck.IsChecked == true);

    private void SaveRepository_Click(object sender, RoutedEventArgs e)
    {
        AppSettingsService.SetGitHubRepositoryUrl(RepositoryTextBox.Text);
        MessageBox.Show("Repository URL saved.", "Harman PC Tools");
    }

    private void OpenRepository_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = AppSettingsService.Current.GitHubRepositoryUrl,
                UseShellExecute = true
            });
        }
        catch
        {
            MessageBox.Show("The repository URL could not be opened.", "Harman PC Tools");
        }
    }
}
