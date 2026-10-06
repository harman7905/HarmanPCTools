using System;
using System.Windows;
using System.Windows.Controls;
using HarmanPCTools.Services;

namespace HarmanPCTools.Pages;

public partial class SystemPage : UserControl
{
    private readonly SystemMetricsService _metrics = new();

    public SystemPage()
    {
        InitializeComponent();
        Loaded += (_, _) => Refresh();
    }

    private void Refresh()
    {
        var s = _metrics.GetSnapshot();
        MachineText.Text = s.MachineName;
        WindowsText.Text = s.WindowsVersion;
        ProcessorText.Text = $"{s.ProcessorCount} logical processor(s)";
        MemoryText.Text = $"{s.FreeMemoryGb:0.0} GB free / {s.TotalMemoryGb:0.0} GB total";
        DriveText.Text = $"{s.DriveFreeGb:0.0} GB free / {s.DriveTotalGb:0.0} GB total";
        GpuText.Text = s.GpuName;
        NetworkText.Text = s.NetworkAvailable ? "Connected" : "Offline";
        UptimeText.Text = s.Uptime;
    }

    private void Copy_Click(object sender, RoutedEventArgs e)
    {
        var s = _metrics.GetSnapshot();
        string report =
            $"Harman PC Tools System Report{Environment.NewLine}" +
            $"Machine: {s.MachineName}{Environment.NewLine}" +
            $"Windows: {s.WindowsVersion}{Environment.NewLine}" +
            $"Processors: {s.ProcessorCount}{Environment.NewLine}" +
            $"GPU: {s.GpuName}{Environment.NewLine}" +
            $"Memory: {s.FreeMemoryGb:0.0} GB free / {s.TotalMemoryGb:0.0} GB total{Environment.NewLine}" +
            $"System Drive: {s.DriveFreeGb:0.0} GB free / {s.DriveTotalGb:0.0} GB total{Environment.NewLine}" +
            $"CPU: {s.CpuUsage:0}%{Environment.NewLine}" +
            $"Memory Usage: {s.MemoryUsage:0}%{Environment.NewLine}" +
            $"Storage Usage: {s.StorageUsage:0}%{Environment.NewLine}" +
            $"Uptime: {s.Uptime}{Environment.NewLine}" +
            $"Network: {(s.NetworkAvailable ? "Connected" : "Offline")}";

        Clipboard.SetText(report);
        MessageBox.Show("System report copied to the clipboard.", "Harman PC Tools");
    }

    private void Device_Click(object sender, RoutedEventArgs e) => WindowsToolsService.Open("devmgmt.msc");
    private void Settings_Click(object sender, RoutedEventArgs e) => WindowsToolsService.Open("ms-settings:about");
}
