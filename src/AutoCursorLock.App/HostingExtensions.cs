namespace AutoCursorLock.App;

using AutoCursorLock.App.Services;
using AutoCursorLock.App.Views;
using AutoCursorLock.Models;
using AutoCursorLock.Native;
using AutoCursorLock.Sdk.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

internal static class HostingExtensions
{
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

    public static ServiceProvider CreateContainer()
    {
        var services = new ServiceCollection();

        services.UseAutoCursorLockApp();

        return services.BuildServiceProvider();
    }
}