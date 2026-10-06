using System.Windows;
using System.Windows.Controls;
using HarmanPCTools.Services;

namespace HarmanPCTools.Pages;

public partial class StartupPage : UserControl
{
    private readonly StartupEntriesService _service = new();

    public StartupPage()
    {
        InitializeComponent();
        Refresh();
    }

    private void Refresh() =>
        StartupGrid.ItemsSource = _service.GetEntries();

    private void Refresh_Click(object sender, RoutedEventArgs e) => Refresh();

    private void TaskManager_Click(object sender, RoutedEventArgs e) =>
        WindowsToolsService.Open("taskmgr.exe", label: "Task Manager");
}
