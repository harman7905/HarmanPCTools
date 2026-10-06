namespace HarmanPCTools.Models;

public sealed class AppSettings
{
    public bool StartWithWindows { get; set; }
    public bool OpenLastPageOnStartup { get; set; }
    public string GitHubRepositoryUrl { get; set; } = "https://github.com/harman7905/HarmanPCTools";
    public string LastOpenedPage { get; set; } = "Home";
}
