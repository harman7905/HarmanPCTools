namespace HarmanPCTools.Models;

public sealed class AppSettings
{
    public bool StartWithWindows { get; set; }
    public string GitHubRepositoryUrl { get; set; } = "https://github.com/";
    public string LastOpenedPage { get; set; } = "Home";
}
