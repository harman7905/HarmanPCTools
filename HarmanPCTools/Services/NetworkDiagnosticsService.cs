using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using HarmanPCTools.Models;

namespace HarmanPCTools.Services;

public sealed class NetworkDiagnosticsService
{
    public async Task<NetworkTestResult> PingAsync(string host)
    {
        try
        {
            using Ping ping = new();

            PingReply reply = await ping.SendPingAsync(host, 3000);

            if (reply.Status == IPStatus.Success)
                return new NetworkTestResult(true, reply.RoundtripTime, $"{host} replied successfully.");

            return new NetworkTestResult(false, 0, $"{host} returned status: {reply.Status}.");
        }
        catch (Exception ex)
        {
            return new NetworkTestResult(false, 0, $"Network test failed: {ex.Message}");
        }
    }
}
