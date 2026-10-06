using System;
using HarmanPCTools.Models;

namespace HarmanPCTools.Services;

public sealed class GamingSessionService
{
    private DateTime? _startedAt;

    public GamingSessionState GetState()
    {
        if (_startedAt is null)
            return new GamingSessionState(false, "00:00:00");

        TimeSpan elapsed = DateTime.Now - _startedAt.Value;
        return new GamingSessionState(true,
            $"{(int)elapsed.TotalHours:00}:{elapsed.Minutes:00}:{elapsed.Seconds:00}");
    }

    public void Start()
    {
        _startedAt ??= DateTime.Now;
    }

    public void Stop()
    {
        _startedAt = null;
    }
}
