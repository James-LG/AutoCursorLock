// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App;

using AutoCursorLock.App.Services;
using AutoCursorLock.App.Views;
using AutoCursorLock.Native;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

/// <summary>
/// Hosting extensions.
/// </summary>
internal static class HostingExtensions
{
    /// <summary>
    /// Registers services for the application.
    /// </summary>
    /// <param name="services">The service collection to add to.</param>
    /// <returns>The service collection with added services.</returns>
    public static IServiceCollection UseAutoCursorLockApp(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.File(Paths.LogPath, retainedFileCountLimit: 5)
            .CreateLogger();

        services.UseAutoCursorLockNative();

        services
            .AddSingleton<MainWindowFactory>()
            .AddSingleton<LoadUserSettingsOperation>()
            .AddSingleton<SaveUserSettingsOperation>()
            .AddLogging(b => b
                .AddSerilog());

        return services;
    }

    /// <summary>
    /// Creates the service provider.
    /// </summary>
    /// <returns>The service provider.</returns>
    public static ServiceProvider CreateContainer()
    {
        var services = new ServiceCollection();

        services.UseAutoCursorLockApp();

        return services.BuildServiceProvider();
    }
}
