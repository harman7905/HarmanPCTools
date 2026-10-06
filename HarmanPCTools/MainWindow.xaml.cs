using System.Windows;
using System.Windows.Controls;
using HarmanPCTools.Pages;

namespace HarmanPCTools;

public partial class MainWindow : Window
{
    private Button? _activeButton;

    public MainWindow()
    {
        InitializeComponent();
        _activeButton = HomeNav;
        PageHost.Content = new HomePage();
    }

    private void Nav_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string tag)
            NavigateTo(tag, button);
    }

    public void NavigateTo(string tag)
    {
        Button? button = FindNavButton(tag);
        NavigateTo(tag, button);
    }

    public void NavigateTo(string tag, Button? button)
    {
        if (button is not null)
            SetActive(button);

        PageHost.Content = tag switch
        {
            "Home" => new HomePage(),
            "Gaming" => new GamingPage(),
            "System" => new SystemPage(),
            "Cleanup" => new CleanupPage(),
            "Organizer" => new OrganizerPage(),
            "Tools" => new ToolsPage(),
            "Startup" => new StartupPage(),
            "Customize" => new CustomizePage(),
            "Fun" => new FunPage(),
            "Settings" => new SettingsPage(),
            "About" => new AboutPage(),
            _ => new HomePage()
        };
    }

    private Button? FindNavButton(string tag) => tag switch
    {
        "Home" => HomeNav,
        "Gaming" => GamingNav,
        "System" => SystemNav,
        "Cleanup" => CleanupNav,
        "Organizer" => OrganizerNav,
        "Tools" => ToolsNav,
        "Startup" => StartupNav,
        "Customize" => CustomizeNav,
        "Fun" => FunNav,
        "Settings" => SettingsNav,
        "About" => AboutNav,
        _ => null
    };

    private void SetActive(Button button)
    {
        if (_activeButton is not null)
            _activeButton.Style = (Style)FindResource("NavButton");

        button.Style = (Style)FindResource("NavButtonActive");
        _activeButton = button;
    }
}
