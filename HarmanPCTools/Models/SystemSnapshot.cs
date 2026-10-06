namespace HarmanPCTools.Models;

public sealed record SystemSnapshot(
    double CpuUsage,
    double MemoryUsage,
    double StorageUsage,
    double FreeMemoryGb,
    double TotalMemoryGb,
    double DriveFreeGb,
    double DriveTotalGb,
    string MachineName,
    string WindowsVersion,
    int ProcessorCount,
    string Uptime,
    bool NetworkAvailable,
    string GpuName);
