// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App.Views;

using System;
using System.Windows;
using AutoCursorLock.App.Models;
using AutoCursorLock.Sdk.Models;

/// <summary>
/// Interaction logic for AppLockSettingsWindow.xaml.
/// </summary>
public partial class AppLockSettingsWindow : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppLockSettingsWindow"/> class.
    /// </summary>
    /// <param name="processListItem">The process to modify settings for.</param>
    public AppLockSettingsWindow(ProcessListItem processListItem)
    {
        AppLockSettings = processListItem;
        InitializeComponent();

        this.mainGrid.DataContext = this;
    }

    /// <summary>
    /// Gets the app lock settings.
    /// </summary>
    public ProcessListItem AppLockSettings { get; init; }

    /// <summary>
    /// Gets the app lock type options.
    /// </summary>
    public AppLockType[] AppLockTypeOptions { get; } = Enum.GetValues<AppLockType>();
}
