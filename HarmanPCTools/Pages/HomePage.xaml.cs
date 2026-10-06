using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using HarmanPCTools.Services;

namespace HarmanPCTools.Pages;

public partial class HomePage : UserControl
{
    private readonly SystemMetricsService _metrics = new();
    private readonly DispatcherTimer _timer;

    public HomePage()
    {
        InitializeComponent();

        GreetingText.Text = GetGreeting();

        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += (_, _) => UpdateMetrics();

        Loaded += (_, _) =>
        {
            UpdateMetrics();
            _timer.Start();
        };

        Unloaded += (_, _) => _timer.Stop();
    }

    private void UpdateMetrics()
    {
        try
        {
            var s = _metrics.GetSnapshot();

            CpuText.Text = $"{s.CpuUsage:0}%";
            MemoryText.Text = $"{s.MemoryUsage:0}%";
            StorageText.Text = $"{s.StorageUsage:0}%";
            FreeRamText.Text = $"{s.FreeMemoryGb:0.0} GB";

            RamDetail.Text = $"of {s.TotalMemoryGb:0.0} GB available";
            CpuBar.Value = s.CpuUsage;
            MemoryBar.Value = s.MemoryUsage;
            StorageBar.Value = s.StorageUsage;

            MachineText.Text = s.MachineName;
            WindowsText.Text = s.WindowsVersion;
            UptimeText.Text = s.Uptime;
            NetworkText.Text = s.NetworkAvailable ? "Connected" : "Offline";
            GpuText.Text = s.GpuName;
            DriveText.Text = $"{s.DriveFreeGb:0.0} GB free / {s.DriveTotalGb:0.0} GB";

            bool stressed = s.CpuUsage >= 90 || s.MemoryUsage >= 92 || s.StorageUsage >= 95;
            HealthText.Text = stressed ? "●  CHECK PC STATUS" : "●  SYSTEM HEALTHY";
            HealthText.Foreground = stressed
                ? (System.Windows.Media.Brush)Application.Current.Resources["WarningBrush"]
                : (System.Windows.Media.Brush)Application.Current.Resources["SuccessBrush"];
        }
        catch
        {
            CpuText.Text = "N/A";
            MemoryText.Text = "N/A";
            StorageText.Text = "N/A";
            FreeRamText.Text = "N/A";
            NetworkText.Text = "Unavailable";
            HealthText.Text = "●  STATUS UNAVAILABLE";
        }
    }

    private static string GetGreeting()
    {
        int hour = DateTime.Now.Hour;
        return hour switch
        {
            < 12 => "GOOD MORNING 👋",
            < 18 => "GOOD AFTERNOON 👋",
            _ => "GOOD EVENING 👋"
        };
    }

    private void Gaming_Click(object sender, RoutedEventArgs e) => Navigate("Gaming");
    private void Cleanup_Click(object sender, RoutedEventArgs e) => Navigate("Cleanup");
    private void System_Click(object sender, RoutedEventArgs e) => Navigate("System");
    private void Network_Click(object sender, RoutedEventArgs e) => WindowsToolsService.Open("ms-settings:network");

    private void DownloadsOrganizer_Click(object sender, RoutedEventArgs e) => Navigate("Organizer");
    private void Tools_Click(object sender, RoutedEventArgs e) => Navigate("Tools");
    private void Startup_Click(object sender, RoutedEventArgs e) => Navigate("Startup");
    private void Customize_Click(object sender, RoutedEventArgs e) => Navigate("Customize");
    private void Fun_Click(object sender, RoutedEventArgs e) => Navigate("Fun");

    private void LaunchGta_Click(object sender, RoutedEventArgs e) => TryLaunch("GTA V", "steam://rungameid/271590");
    private void LaunchObs_Click(object sender, RoutedEventArgs e)
    {
        if (!ObsLaunchService.TryLaunchWithPrompt(out string? error))
            MessageBox.Show(error ?? "OBS Studio could not be launched.", "Harman PC Toolkit");
    }

    private void Downloads_Click(object sender, RoutedEventArgs e)
    {
        OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads", "Downloads");
    }

    private void Screenshots_Click(object sender, RoutedEventArgs e)
    {
        OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Pictures\\Screenshots", "Screenshots");
    }

    private void Navigate(string tag)
    {
        if (Application.Current.MainWindow is MainWindow window)
            window.NavigateTo(tag);
    }

    private static void TryLaunch(string name, string target)
    {
        try
        {
            Process.Start(new ProcessStartInfo { FileName = target, UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not launch {name}.\n\n{ex.Message}", "Harman PC Toolkit");
        }
    }

    private static void OpenFolder(string path, string label)
    {
        try
        {
            Process.Start(new ProcessStartInfo { FileName = path, UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not open {label}.\n\n{ex.Message}", "Harman PC Toolkit");
        }
    }
}
