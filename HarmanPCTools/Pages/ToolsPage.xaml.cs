using System;
using System.Windows;
using System.Windows.Controls;
using HarmanPCTools.Services;

namespace HarmanPCTools.Pages;

public partial class ToolsPage : UserControl
{
    private readonly NetworkDiagnosticsService _network = new();

    public ToolsPage() => InitializeComponent();

    private void TaskManager_Click(object sender, RoutedEventArgs e) =>
        WindowsToolsService.Open("taskmgr.exe");

    private void Device_Click(object sender, RoutedEventArgs e) =>
        WindowsToolsService.Open("devmgmt.msc");

    private void Event_Click(object sender, RoutedEventArgs e) =>
        WindowsToolsService.Open("eventvwr.msc");

    private void Services_Click(object sender, RoutedEventArgs e) =>
        WindowsToolsService.Open("services.msc");

    private void Disk_Click(object sender, RoutedEventArgs e) =>
        WindowsToolsService.Open("diskmgmt.msc");

    private void Control_Click(object sender, RoutedEventArgs e) =>
        WindowsToolsService.Open("control.exe");

    private void Cmd_Click(object sender, RoutedEventArgs e) =>
        WindowsToolsService.Open("cmd.exe");

    private void PowerShell_Click(object sender, RoutedEventArgs e) =>
        WindowsToolsService.Open("powershell.exe");

    private void Network_Click(object sender, RoutedEventArgs e) =>
        WindowsToolsService.OpenSettings("ms-settings:network", "Network Settings");

    private void Display_Click(object sender, RoutedEventArgs e) =>
        WindowsToolsService.OpenSettings("ms-settings:display", "Display Settings");

    private void Sound_Click(object sender, RoutedEventArgs e) =>
        WindowsToolsService.OpenSettings("ms-settings:sound", "Sound Settings");

    private void Update_Click(object sender, RoutedEventArgs e) =>
        WindowsToolsService.OpenSettings("ms-settings:windowsupdate", "Windows Update");

    private async void Ping_Click(object sender, RoutedEventArgs e)
    {
        string host = HostTextBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(host))
        {
            PingResultText.Text = "Enter a host or IP address first.";
            return;
        }

        PingResultText.Text = $"Testing {host}...";

        var result = await _network.PingAsync(host);

        PingResultText.Text = result.Success
            ? $"✓ {result.Message}  •  {result.Milliseconds} ms"
            : $"✕ {result.Message}";
    }
}
