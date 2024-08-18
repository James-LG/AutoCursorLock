namespace AutoCursorLock.Native;

using Microsoft.Extensions.DependencyInjection;
using System;

public static class HostingExtensions
{
    public static IServiceCollection UseAutoCursorLockNative(this IServiceCollection services)
    {
        services.AddSingleton<ApplicationHandler>();

        return services;
    }
}