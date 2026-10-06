using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;
using HarmanPCTools.Models;

namespace HarmanPCTools.Services;

public sealed class SystemMetricsService
{
    [StructLayout(LayoutKind.Sequential)]
    private struct FILETIME
    {
        public uint dwLowDateTime;
        public uint dwHighDateTime;
    }

    [DllImport("kernel32.dll")]
    private static extern bool GetSystemTimes(
        out FILETIME idleTime,
        out FILETIME kernelTime,
        out FILETIME userTime);

    [StructLayout(LayoutKind.Sequential)]
    private struct MEMORYSTATUSEX
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

    [DllImport("kernel32.dll")]
    private static extern ulong GetTickCount64();

    private ulong _previousIdle;
    private ulong _previousKernel;
    private ulong _previousUser;
    private bool _hasCpuSample;

    public SystemSnapshot GetSnapshot()
    {
        double cpu = GetCpuUsage();
        double memoryUsage = 0;
        double freeMemoryGb = 0;
        double totalMemoryGb = 0;

        MEMORYSTATUSEX memory = new() { dwLength = (uint)Marshal.SizeOf<MEMORYSTATUSEX>() };
        if (GlobalMemoryStatusEx(ref memory))
        {
            memoryUsage = memory.dwMemoryLoad;
            totalMemoryGb = memory.ullTotalPhys / 1024d / 1024d / 1024d;
            freeMemoryGb = memory.ullAvailPhys / 1024d / 1024d / 1024d;
        }

        DriveInfo? drive = GetSystemDrive();
        double storageUsage = 0;
        double driveFreeGb = 0;
        double driveTotalGb = 0;

        if (drive is not null)
        {
            driveTotalGb = drive.TotalSize / 1024d / 1024d / 1024d;
            driveFreeGb = drive.AvailableFreeSpace / 1024d / 1024d / 1024d;
            if (driveTotalGb > 0)
                storageUsage = Math.Clamp((1 - (driveFreeGb / driveTotalGb)) * 100, 0, 100);
        }

        return new SystemSnapshot(
            Math.Round(cpu, 0),
            Math.Round(memoryUsage, 0),
            Math.Round(storageUsage, 0),
            Math.Round(freeMemoryGb, 1),
            Math.Round(totalMemoryGb, 1),
            Math.Round(driveFreeGb, 1),
            Math.Round(driveTotalGb, 1),
            Environment.MachineName,
            Environment.OSVersion.VersionString,
            Environment.ProcessorCount,
            FormatUptime(TimeSpan.FromMilliseconds(GetTickCount64())),
            NetworkInterface.GetIsNetworkAvailable(),
            GetGpuName());
    }

    private double GetCpuUsage()
    {
        if (!GetSystemTimes(out FILETIME idle, out FILETIME kernel, out FILETIME user))
            return 0;

        ulong idleNow = ToUInt64(idle);
        ulong kernelNow = ToUInt64(kernel);
        ulong userNow = ToUInt64(user);

        if (!_hasCpuSample)
        {
            _previousIdle = idleNow;
            _previousKernel = kernelNow;
            _previousUser = userNow;
            _hasCpuSample = true;
            Thread.Sleep(80);

            if (!GetSystemTimes(out idle, out kernel, out user))
                return 0;

            idleNow = ToUInt64(idle);
            kernelNow = ToUInt64(kernel);
            userNow = ToUInt64(user);
        }

        ulong idleDelta = idleNow - _previousIdle;
        ulong kernelDelta = kernelNow - _previousKernel;
        ulong userDelta = userNow - _previousUser;

        _previousIdle = idleNow;
        _previousKernel = kernelNow;
        _previousUser = userNow;

        ulong total = kernelDelta + userDelta;
        if (total == 0)
            return 0;

        double usage = (1d - ((double)idleDelta / total)) * 100d;
        return Math.Clamp(usage, 0, 100);
    }

    private static ulong ToUInt64(FILETIME time) =>
        ((ulong)time.dwHighDateTime << 32) | time.dwLowDateTime;

    private static DriveInfo? GetSystemDrive()
    {
        string root = Path.GetPathRoot(Environment.SystemDirectory) ?? "C:\\";
        try { return new DriveInfo(root); }
        catch { return null; }
    }

    private static string FormatUptime(TimeSpan uptime)
    {
        if (uptime.TotalDays >= 1)
            return $"{(int)uptime.TotalDays}d {uptime.Hours}h {uptime.Minutes}m";
        if (uptime.TotalHours >= 1)
            return $"{(int)uptime.TotalHours}h {uptime.Minutes}m";
        return $"{Math.Max(0, uptime.Minutes)}m";
    }

    private static string GetGpuName()
    {
        try
        {
            object? value = Registry.GetValue(
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\WinSAT",
                "PrimaryAdapterString",
                null);

            string? name = value?.ToString();
            if (!string.IsNullOrWhiteSpace(name))
                return name.Trim();
        }
        catch
        {
        }

        return "Graphics adapter";
    }
}
