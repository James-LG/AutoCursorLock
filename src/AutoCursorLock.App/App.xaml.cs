// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App;

using AutoCursorLock.App.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

/// <summary>
/// Interaction logic for App.xaml.
/// </summary>
public partial class App : Application
{
    public static ServiceProvider Services { get; private set; } = HostingExtensions.CreateContainer();

    public App()
    {
        var currentDomain = AppDomain.CurrentDomain;
        currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler);
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        var mainWindowFactory = Services.GetRequiredService<MainWindowFactory>();
        var mainWindow = await mainWindowFactory.CreateAsync();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Services.Dispose();

        base.OnExit(e);
    }

    static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
    {
        var ex = (Exception)e.ExceptionObject;

        // Write the exception message to an error log file.
        System.IO.File.WriteAllText("error.log", ex.ToString());

        // Show a message box with the exception message.
        MessageBox.Show($"An unhandled exception occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        // Exit the application.
        Environment.Exit(1);
    }
}
