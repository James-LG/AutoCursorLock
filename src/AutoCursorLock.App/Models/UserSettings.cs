// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App.Models;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AutoCursorLock.Native;
using Serilog.Core;
using Serilog.Events;

/// <summary>
/// Represents the program settings as defined by the user.
/// </summary>
public class UserSettings : INotifyPropertyChanged
{
    private HotKey? hotKey;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserSettings"/> class.
    /// </summary>
    /// <param name="enabledProcesses">The processes enabled byh the user.</param>
    /// <param name="hotKey">The hot key to toggle the cursor lock.</param>
    /// <param name="loggingSwitch">The logging level for the application.</param>
    public UserSettings(ObservableCollection<ProcessListItem> enabledProcesses, HotKey? hotKey, LoggingLevelSwitch loggingSwitch)
    {
        EnabledProcesses = enabledProcesses ?? throw new ArgumentNullException(nameof(enabledProcesses));
        HotKey = hotKey;
        LoggingSwitch = loggingSwitch;
    }

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Gets the processes that the user has selected to lock the cursor on when in focus.
    /// </summary>
    /// <remarks>
    /// When a window from an enabled process is focused by the user, this program will lock the cursor
    /// to the bounds of the window until focus is changed to another process.
    /// </remarks>
    public ObservableCollection<ProcessListItem> EnabledProcesses { get; }

    /// <summary>
    /// Gets or sets the hot key used to toggle the programs locking feature.
    /// </summary>
    public HotKey? HotKey
    {
        get
        {
            return this.hotKey;
        }

        set
        {
            this.hotKey = value;
            NotifyPropertyChanged(nameof(HotKey));
        }
    }

    /// <summary>
    /// Gets or sets the logging level for the application.
    /// </summary>
    public LoggingLevelSwitch LoggingSwitch { get; set; }

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
