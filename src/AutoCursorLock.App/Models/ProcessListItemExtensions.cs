// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App.Models;

using AutoCursorLock.Sdk.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

/// <summary>
/// Extensions for <see cref="ProcessListItem"/>.
/// </summary>
internal static class ProcessListItemExtensions
{
    /// <summary>
    /// Creates a <see cref="ProcessListItem"/> from a path.
    /// </summary>
    /// <param name="path">The process' path.</param>
    /// <returns>The process list item.</returns>
    /// <exception cref="AutoCursorLockException">If the process could not be found.</exception>
    public static ProcessListItem FromPath(string path)
    {
        var process = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(path))
            .FirstOrDefault()
            ?? throw new AutoCursorLockException($"Could not find process for path: {path}");

        return FromAppLockSettings(new AppLockSettingsModel(process.ProcessName, path, AppLockType.Window, Margin: default, ReapplyLockInterval: default));
    }

    /// <summary>
    /// Creates a <see cref="ProcessListItem"/> from a name and path.
    /// </summary>
    /// <param name="appLockSettings">The app lock settings.</param>
    /// <returns>The process list item.</returns>
    public static ProcessListItem FromAppLockSettings(AppLockSettingsModel appLockSettings)
    {
        var bitmapIcon = default(BitmapSource?);
        if (appLockSettings.Path is not null && File.Exists(appLockSettings.Path))
        {
            using var icon = System.Drawing.Icon.ExtractAssociatedIcon(appLockSettings.Path);
            bitmapIcon = icon is null
                ? null
                : Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
        else
        {
            bitmapIcon = new BitmapImage(new Uri("pack://application:,,,/media/question-mark.png", UriKind.Absolute));
        }

        return new ProcessListItem(appLockSettings.Name, appLockSettings.Path, appLockSettings.Type, appLockSettings.Margin, appLockSettings.ReapplyLockInterval, bitmapIcon);
    }

    /// <summary>
    /// Converts a <see cref="ProcessListItem"/> to a <see cref="AppLockSettingsModel"/>.
    /// </summary>
    /// <param name="processListItem">The process list item.</param>
    /// <returns>The model.</returns>
    public static AppLockSettingsModel ToModel(this ProcessListItem processListItem)
    {
        return new AppLockSettingsModel(
            processListItem.Name,
            processListItem.Path,
            processListItem.AppLockType,
            processListItem.Margin,
            processListItem.ReapplyLockInterval
        );
    }

    /// <summary>
    /// Converts a <see cref="AppLockSettingsModel"/> to a <see cref="ProcessListItem"/>.
    /// </summary>
    /// <param name="appLockSettingsModel">The model.</param>
    /// <returns>The process list item.</returns>
    public static ProcessListItem ToViewModel(AppLockSettingsModel appLockSettingsModel)
    {
        return FromAppLockSettings(appLockSettingsModel);
    }
}
