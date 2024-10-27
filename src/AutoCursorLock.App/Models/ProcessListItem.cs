// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App.Models;

using AutoCursorLock.Sdk.Models;
using System;
using System.Windows.Media.Imaging;

/// <summary>
/// Represents a process to be displayed in the process selection list.
/// </summary>
public record ProcessListItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessListItem"/> class.
    /// </summary>
    /// <param name="name">The name of the process.</param>
    /// <param name="path">The path of the proceess' executable.</param>
    /// <param name="appLockType">The type of app lock.</param>
    /// <param name="margin">The margin around the window for the app lock.</param>
    /// <param name="icon">The icon image of the process.</param>
    public ProcessListItem(string name, string? path, AppLockType appLockType, AppLockMargin? margin, BitmapSource? icon)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Path = path;
        AppLockType = appLockType;
        Margin = margin ?? new AppLockMargin(0, 0, 0, 0);
        Icon = icon;
    }

    /// <summary>
    /// Gets the name of the process.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the path of the process' executable.
    /// </summary>
    public string? Path { get; }

    /// <summary>
    /// Gets the type of app lock.
    /// </summary>
    public AppLockType AppLockType { get; init; }

    /// <summary>
    /// Gets the margin around the window for the app lock.
    /// </summary>
    public AppLockMargin Margin { get; init; }

    /// <summary>
    /// Gets the icon image of the process.
    /// </summary>
    public BitmapSource? Icon { get; }
}
