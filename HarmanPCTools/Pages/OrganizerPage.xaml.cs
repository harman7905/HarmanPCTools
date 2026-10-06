using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HarmanPCTools.Services;

namespace HarmanPCTools.Pages;

public partial class OrganizerPage : UserControl
{
    private readonly OrganizerService _organizer = new();

    public OrganizerPage()
    {
        InitializeComponent();
        PathText.Text = _organizer.DownloadsPath;
        Preview();
    }

    private void Preview()
    {
        var preview = _organizer.GetPreview();
        PreviewText.Text = preview.Count == 0
            ? "No files found in the top level of Downloads."
            : string.Join("\n", preview.Select(item => $"{item.Key}: {item.Value} file(s)"));
    }

    private void Preview_Click(object sender, RoutedEventArgs e) => Preview();

    private void Organize_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show(
            "Organize the files currently in your Downloads folder?\n\nExisting files will be preserved by renaming the moved copy.",
            "Harman PC Tools",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes) return;

        int moved = _organizer.Organize();
        Preview();

        MessageBox.Show(
            $"Organizer finished.\n\nFiles moved: {moved}",
            "Harman PC Tools");
    }
}
