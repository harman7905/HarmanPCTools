using System.Windows;
using System.Windows.Controls;
using HarmanPCTools.Services;

namespace HarmanPCTools.Pages;

public partial class CleanupPage : UserControl
{
    private readonly CleanupService _cleanup = new();

    public CleanupPage()
    {
        InitializeComponent();
        TempPathText.Text = _cleanup.UserTempPath;
        Loaded += (_, _) => Scan();
    }

    private void Scan()
    {
        TempSizeText.Text =
            CleanupService.FormatBytes(_cleanup.CalculateTempSize());
    }

    private void Scan_Click(object sender, RoutedEventArgs e) => Scan();

    private void Clean_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show(
            "Clean the current user's temporary folder?\n\nFiles currently in use will be skipped.",
            "Harman PC Toolkit",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes)
            return;

        int count = _cleanup.DeleteUserTempFiles(out long freedBytes);
        Scan();

        MessageBox.Show(
            $"Cleanup finished.\n\nItems removed: {count}\nSpace freed: {CleanupService.FormatBytes(freedBytes)}",
            "Harman PC Toolkit");
    }

    private void Storage_Click(object sender, RoutedEventArgs e) =>
        WindowsToolsService.OpenSettings("ms-settings:storagesense", "Storage Settings");

    private void DiskCleanup_Click(object sender, RoutedEventArgs e) =>
        WindowsToolsService.Open("cleanmgr.exe");
}
