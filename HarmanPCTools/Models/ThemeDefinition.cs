namespace HarmanPCTools.Models;

public sealed record ThemeDefinition(
    string Name,
    string Background,
    string Sidebar,
    string Panel,
    string Panel2,
    string Border,
    string Text,
    string Muted,
    string Accent,
    string AccentSoft,
    string NavHover,
    string CardHover,
    string SecondaryHover,
    string PrimaryHover,
    string Success,
    string Warning,
    string Danger);
