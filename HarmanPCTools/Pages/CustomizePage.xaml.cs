using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HarmanPCTools.Models;
using HarmanPCTools.Services;

namespace HarmanPCTools.Pages;

public partial class CustomizePage : UserControl
{
    public CustomizePage()
    {
        InitializeComponent();
        BuildThemeButtons();
        ShowCurrentTheme();
    }

    private void BuildThemeButtons()
    {
        ThemePanel.Children.Clear();

        foreach (ThemeDefinition theme in ThemeService.Themes)
        {
            Color panelColor = (Color)ColorConverter.ConvertFromString(theme.Panel)!;
            Color accentColor = (Color)ColorConverter.ConvertFromString(theme.Accent)!;

            StackPanel content = new()
            {
                Orientation = Orientation.Vertical
            };

            Border swatch = new()
            {
                Height = 8,
                CornerRadius = new CornerRadius(4),
                Background = new SolidColorBrush(accentColor),
                Margin = new Thickness(0, 0, 0, 10)
            };

            TextBlock name = new()
            {
                Text = theme.Name,
                Foreground = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString(theme.Text)!),
                FontSize = 13,
                FontWeight = FontWeights.SemiBold
            };

            TextBlock description = new()
            {
                Text = "Dark • Professional",
                Foreground = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString(theme.Muted)!),
                FontSize = 10,
                Margin = new Thickness(0, 3, 0, 0)
            };

            content.Children.Add(swatch);
            content.Children.Add(name);
            content.Children.Add(description);

            Button button = new()
            {
                Content = content,
                Tag = theme,
                Width = 180,
                Height = 84,
                Margin = new Thickness(0, 0, 10, 10),
                Padding = new Thickness(13),
                Background = new SolidColorBrush(panelColor),
                Foreground = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString(theme.Text)!),
                BorderBrush = new SolidColorBrush(accentColor),
                BorderThickness = new Thickness(1),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            button.Click += Theme_Click;
            ThemePanel.Children.Add(button);
        }
    }

    private void Theme_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is ThemeDefinition theme)
        {
            ThemeService.ApplyTheme(theme);
            ShowCurrentTheme();
        }
    }

    private void ShowCurrentTheme()
    {
        ThemeNameText.Text = ThemeService.Current.Name;
        ThemeSwatch.Background = new SolidColorBrush(
            (Color)ColorConverter.ConvertFromString(ThemeService.Current.Accent)!);
    }
}
