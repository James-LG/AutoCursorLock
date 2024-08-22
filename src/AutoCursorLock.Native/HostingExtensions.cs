// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Native;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions for <see cref="IServiceCollection"/>.
/// </summary>
public static class HostingExtensions
{
    /// <summary>
    /// Adds services required for the native library.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection with the added services.</returns>
    public static IServiceCollection UseAutoCursorLockNative(this IServiceCollection services)
    {
        services.AddSingleton<ApplicationEventSource>();

        return services;
    }
}
