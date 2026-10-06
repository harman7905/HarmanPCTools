namespace HarmanPCTools.Models;

public sealed record NetworkTestResult(bool Success, long Milliseconds, string Message);
