using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32;
using HarmanPCTools.Models;
using HarmanPCTools.Services;

namespace HarmanPCTools.Pages;

public partial class GamingPage : UserControl
{
    private readonly GameShortcutService _games = new();
    private readonly GamingSessionService _session = new();
    private readonly SystemMetricsService _metrics = new();
    private readonly DispatcherTimer _timer;

    public GamingPage()
    {
        InitializeComponent();

        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += (_, _) =>
        {
            UpdateSession();
            UpdateReadiness();
        };

        Loaded += (_, _) =>
        {
            RefreshGames();
            UpdateSession();
            UpdateReadiness();
            _timer.Start();
        };

        Unloaded += (_, _) => _timer.Stop();
    }

    private void UpdateSession() =>
        SessionTimerText.Text = _session.GetState().Elapsed;

    private void StartSession_Click(object sender, RoutedEventArgs e)
    {
        _session.Start();
        UpdateSession();
    }

    private void StopSession_Click(object sender, RoutedEventArgs e)
    {
        _session.Stop();
        UpdateSession();
    }

    private void LaunchWorkspace_Click(object sender, RoutedEventArgs e)
    {
        bool launchedSomething = false;

        if (SteamCheck.IsChecked == true)
        {
            bool ok = ApplicationLauncherService.TryLaunch(
                "steam://open/games", "Steam", out string? error);

            launchedSomething |= ok;
            if (!ok)
                ShowWarning(error);
        }

        if (ObsCheck.IsChecked == true)
        {
            bool ok = ObsLaunchService.TryLaunchWithPrompt(out string? error);

            launchedSomething |= ok;
            if (!ok)
                ShowWarning(error);
        }

        if (launchedSomething)
        {
            _session.Start();
            UpdateSession();
        }
    }

    private static void ShowWarning(string? message)
    {
        MessageBox.Show(
            message ?? "The application could not be launched.",
            "Harman PC Tools",
            MessageBoxButton.OK,
            MessageBoxImage.Warning);
    }

    private void RefreshGames()
    {
        GamesPanel.Children.Clear();

        if (_games.Games.Count == 0)
        {
            GamesPanel.Children.Add(new TextBlock
            {
                Text = "No pinned games yet.",
                Foreground = (Brush)Application.Current.Resources["MutedBrush"],
                FontSize = 12,
                Margin = new Thickness(0, 0, 0, 12)
            });
            return;
        }

        foreach (GameShortcut game in _games.Games)
        {
            Border card = new()
            {
                Background = (Brush)Application.Current.Resources["Panel2Brush"],
                BorderBrush = (Brush)Application.Current.Resources["BorderBrush"],
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(10),
                Margin = new Thickness(0, 0, 0, 8)
            };

            Grid grid = new();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            StackPanel info = new();
            info.Children.Add(new TextBlock
            {
                Text = game.Name,
                Foreground = (Brush)Application.Current.Resources["TextBrush"],
                FontSize = 13,
                FontWeight = FontWeights.SemiBold
            });
            info.Children.Add(new TextBlock
            {
                Text = game.Path,
                Foreground = (Brush)Application.Current.Resources["MutedBrush"],
                FontSize = 10,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 3, 12, 0)
            });

            StackPanel actions = new() { Orientation = Orientation.Horizontal };

            Button launch = new()
            {
                Content = "Launch",
                Tag = game,
                Style = (Style)FindResource("PrimaryButton"),
                Margin = new Thickness(0, 0, 6, 0)
            };
            launch.Click += LaunchGame_Click;

            Button remove = new()
            {
                Content = "Remove",
                Tag = game,
                Style = (Style)FindResource("SecondaryButton")
            };
            remove.Click += RemoveGame_Click;

            actions.Children.Add(launch);
            actions.Children.Add(remove);

            Grid.SetColumn(info, 0);
            Grid.SetColumn(actions, 1);
            grid.Children.Add(info);
            grid.Children.Add(actions);

            card.Child = grid;
            GamesPanel.Children.Add(card);
        }
    }

    private void AddGame_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new()
        {
            Title = "Select a game executable",
            Filter = "Applications (*.exe)|*.exe",
            Multiselect = false
        };

        if (dialog.ShowDialog() != true)
            return;

        string defaultName = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName);
        string? name = AskForName(defaultName);

        if (string.IsNullOrWhiteSpace(name))
            return;

        _games.Add(name, dialog.FileName);
        RefreshGames();
    }

    private void LaunchGame_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button || button.Tag is not GameShortcut game)
            return;

        if (!_games.Launch(game, out string? error))
        {
            ShowWarning(error);
            return;
        }

        _session.Start();
        UpdateSession();
    }

    private void RemoveGame_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is GameShortcut game)
        {
            _games.Remove(game);
            RefreshGames();
        }
    }

    private void Steam_Click(object sender, RoutedEventArgs e) =>
        ShowLaunchWarning(ApplicationLauncherService.TryLaunch(
            "steam://open/games", "Steam", out string? error), error);

    private void Epic_Click(object sender, RoutedEventArgs e) =>
        ShowLaunchWarning(ApplicationLauncherService.TryLaunch(
            "com.epicgames.launcher://apps", "Epic Games", out string? error), error);

    private void Obs_Click(object sender, RoutedEventArgs e) =>
        ShowLaunchWarning(ObsLaunchService.TryLaunchWithPrompt(out string? error), error);

    private void Nvidia_Click(object sender, RoutedEventArgs e) =>
        ShowLaunchWarning(ApplicationLauncherService.TryLaunchNvidiaControlPanel(out string? error), error);

    private static void ShowLaunchWarning(bool success, string? error)
    {
        if (!success)
            ShowWarning(error);
    }

    private void GameMode_Click(object sender, RoutedEventArgs e) =>
        WindowsToolsService.Open("ms-settings:gaming-gamemode");

    private void Display_Click(object sender, RoutedEventArgs e) =>
        WindowsToolsService.Open("ms-settings:display");

    private void Graphics_Click(object sender, RoutedEventArgs e) =>
        WindowsToolsService.Open("ms-settings:display-advanced");

    private void Network_Click(object sender, RoutedEventArgs e) =>
        WindowsToolsService.Open("ms-settings:network");

    private void TaskManager_Click(object sender, RoutedEventArgs e) =>
        WindowsToolsService.Open("taskmgr.exe");

    private void ConfigureObs_Click(object sender, RoutedEventArgs e)
    {
        if (ObsLaunchService.TryConfigure(out string? error))
        {
            MessageBox.Show("OBS Studio path saved.", "Harman PC Tools");
        }
        else if (!string.IsNullOrWhiteSpace(error) &&
                 !error.Contains("cancelled", StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show(error, "Harman PC Tools");
        }
    }

    private void UpdateReadiness()
    {
        try
        {
            var s = _metrics.GetSnapshot();
            bool ready = s.MemoryUsage < 90 && s.StorageUsage < 95 && s.NetworkAvailable;

            ReadyText.Text = ready ? "GAME READY" : "CHECK PC";
            ReadyText.Foreground = ready
                ? (Brush)Application.Current.Resources["TextBrush"]
                : (Brush)Application.Current.Resources["WarningBrush"];
        }
        catch
        {
            ReadyText.Text = "STATUS UNKNOWN";
        }
    }

    private static string? AskForName(string defaultName)
    {
        Window dialog = new()
        {
            Title = "Add Game",
            Width = 420,
            Height = 175,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            ResizeMode = ResizeMode.NoResize,
            Background = (Brush)Application.Current.Resources["PanelBrush"],
            Owner = Application.Current.MainWindow
        };

        Grid grid = new() { Margin = new Thickness(18) };
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        TextBlock label = new()
        {
            Text = "Game name",
            Foreground = (Brush)Application.Current.Resources["TextBrush"],
            FontSize = 13
        };

        TextBox input = new()
        {
            Text = defaultName,
            Margin = new Thickness(0, 8, 0, 12),
            Padding = new Thickness(8),
            FontSize = 13
        };

        StackPanel actions = new() { Orientation = Orientation.Horizontal };

        Button ok = new()
        {
            Content = "Add",
            Width = 90,
            Style = (Style)Application.Current.FindResource("PrimaryButton")
        };

        Button cancel = new()
        {
            Content = "Cancel",
            Width = 90,
            Margin = new Thickness(8, 0, 0, 0),
            Style = (Style)Application.Current.FindResource("SecondaryButton")
        };

        ok.Click += (_, _) => { dialog.DialogResult = true; dialog.Close(); };
        cancel.Click += (_, _) => { dialog.DialogResult = false; dialog.Close(); };

        actions.Children.Add(ok);
        actions.Children.Add(cancel);

        Grid.SetRow(label, 0);
        Grid.SetRow(input, 1);
        Grid.SetRow(actions, 2);

        grid.Children.Add(label);
        grid.Children.Add(input);
        grid.Children.Add(actions);

        dialog.Content = grid;
        dialog.Loaded += (_, _) => input.Focus();
        input.SelectAll();

        return dialog.ShowDialog() == true ? input.Text.Trim() : null;
    }

    private void Refresh_Click(object sender, RoutedEventArgs e) =>
        RefreshGames();
}
