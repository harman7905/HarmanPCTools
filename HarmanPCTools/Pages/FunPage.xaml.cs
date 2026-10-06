using System;
using System.Windows;
using System.Windows.Controls;

namespace HarmanPCTools.Pages;

public partial class FunPage : UserControl
{
    private readonly string[] _moods =
    {
        "Your PC says: let's get things done. 🚀",
        "Gaming mode energy detected. 🎮",
        "Clean desktop. Clean mind. ✨",
        "Your SSD approves of this application. 💾",
        "CPU is calm. You can probably open another tab. 😄",
        "Harman PC Toolkit recommends a snack break. ☕"
    };

    private readonly Random _random = new();

    public FunPage() => InitializeComponent();

    private void Surprise_Click(object sender, RoutedEventArgs e) =>
        MoodText.Text = _moods[_random.Next(_moods.Length)];
}
