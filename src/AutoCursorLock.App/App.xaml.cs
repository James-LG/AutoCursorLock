// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App;

using AutoCursorLock.App.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;

/// <summary>
/// Interaction logic for App.xaml.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// </summary>
    public App()
    {
        var currentDomain = AppDomain.CurrentDomain;
        currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler);
    }

    /// <summary>
    /// Gets the service provider.
    /// </summary>
    public static ServiceProvider Services { get; private set; } = HostingExtensions.CreateContainer();

    private MinimizeToTray? minimizeToTray;

    /// <summary>
    /// Handles unhandled exceptions.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">Event args.</param>
    protected static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
    {
        var ex = (Exception)e.ExceptionObject;

        // Write the exception message to an error log file.
        System.IO.File.WriteAllText("error.log", ex.ToString());

        // Show a message box with the exception message.
        MessageBox.Show($"An unhandled exception occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        // Exit the application.
        Environment.Exit(1);
    }

    /// <summary>
    /// Starts the application.
    /// </summary>
    /// <param name="e">Event args.</param>
    protected override async void OnStartup(StartupEventArgs e)
    {
        var mainWindowFactory = Services.GetRequiredService<MainWindowFactory>();
        var mainWindow = await mainWindowFactory.CreateAsync();

        this.minimizeToTray = new MinimizeToTray(mainWindow);

        // get arguments
        var minimizeArg = e.Args.FirstOrDefault(arg => arg == "--minimize") is not null;
        if (minimizeArg)
        {
            var quietArg = e.Args.FirstOrDefault(arg => arg == "--quiet") is not null;

            mainWindow.WindowState = WindowState.Minimized;
            this.minimizeToTray.UpdateTrayState(showBalloon: !quietArg);
        }
        else
        {
            mainWindow.Show();
        }

        this.minimizeToTray.StartWatching();

        base.OnStartup(e);
    }

    /// <summary>
    /// Exits the application.
    /// </summary>
    /// <param name="e">Event args.</param>
    protected override void OnExit(ExitEventArgs e)
    {
        Services.Dispose();

        base.OnExit(e);
    }
}
