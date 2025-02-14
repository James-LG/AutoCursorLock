// Copyright(c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App.Views;

using AutoCursorLock.App.Models;
using AutoCursorLock.Sdk.Models;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.Windows;

/// <summary>
/// Interaction logic for GeneralSettings.xaml.
/// </summary>
public partial class GeneralSettingsWindow : Window
{
    private readonly string shortcutPath;

    private readonly UserSettings userSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="GeneralSettingsWindow"/> class.
    /// </summary>
    /// <param name="userSettings">The user settings model.</param>
    public GeneralSettingsWindow(UserSettings userSettings)
    {
        InitializeComponent();

        // check if the shortcut exists in the startup folder
        var startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        this.shortcutPath = System.IO.Path.Combine(startupFolder, "AutoCursorLock.lnk");
        StartWithWindows = System.IO.File.Exists(this.shortcutPath);

        this.mainGrid.DataContext = this;
        this.userSettings = userSettings;
    }

    /// <summary>
    /// Gets or sets a value indicating whether to start the application when Windows starts.
    /// </summary>
    public bool StartWithWindows { get; set; }

    /// <summary>
    /// Gets or sets the logging level for the application.
    /// </summary>
    public LogEventLevel LogLevel
    {
        get => this.userSettings.LoggingSwitch.MinimumLevel;
        set => this.userSettings.LoggingSwitch.MinimumLevel = value;
    }

    /// <summary>
    /// Gets the log event levels.
    /// </summary>
    public LogEventLevel[] LogEventLevels => Enum.GetValues<LogEventLevel>();

    private void StartWithWindowsCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        if (System.IO.File.Exists(this.shortcutPath))
        {
            return;
        }

        // create a shortcut in the startup folder
        var shortcutPath = this.shortcutPath;
        var targetPath = Process.GetCurrentProcess().MainModule?.FileName ?? throw new AutoCursorLockException("Could not get entry assembly");

        var shellType = Type.GetTypeFromProgID("WScript.Shell") ?? throw new AutoCursorLockException("Could not get WScript.Shell type");

        // The use of dynamic here is unfortunate, but using the Activator removes a dependency on the IWshRuntimeLibrary
        // and even if we used the full library, the IWshShell3.CreateShortctut method returns a dynamic...
        dynamic shell = Activator.CreateInstance(shellType) ?? throw new AutoCursorLockException("Could not create WScript.Shell instance");

        var shortcut = shell.CreateShortcut(shortcutPath);
        shortcut.Description = "AutoCursorLock";
        shortcut.TargetPath = targetPath;
        shortcut.Arguments = "--minimize --quiet";
        shortcut.Save();
    }

    private void StartWithWindowsCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        // delete the shortcut in the startup folder
        var shortcutPath = this.shortcutPath;
        if (System.IO.File.Exists(shortcutPath))
        {
            System.IO.File.Delete(shortcutPath);
        }
    }
}
