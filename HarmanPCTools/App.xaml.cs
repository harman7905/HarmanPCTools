using System.Windows;
using HarmanPCTools.Services;

namespace HarmanPCTools;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        AppSettingsService.Load();
        ThemeService.LoadSavedTheme();

        MainWindow mainWindow = new();
        MainWindow = mainWindow;
        mainWindow.Show();
    }
}
