// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App.Models;

using AutoCursorLock.Sdk.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

/// <summary>
/// Extensions for <see cref="UserSettings"/>.
/// </summary>
public static class UserSettingsExtensions
{
    /// <summary>
    /// Converts a <see cref="UserSettingsModel"/> to a <see cref="UserSettings"/>.
    /// </summary>
    /// <param name="userSettingsModel">The model.</param>
    /// <returns>The view model.</returns>
    public static ConversionResult<UserSettings> ToViewModel(this UserSettingsModel userSettingsModel)
    {
        var failures = new List<string>();
        var processes = new ObservableCollection<ProcessListItem>();
        foreach (var appLock in userSettingsModel.AppLocks)
        {
            if (File.Exists(appLock.Path))
            {
                try
                {
                    var processListItem = ProcessListItemExtensions.FromNameAndPath(appLock.Name, appLock.Path);
                    processes.Add(processListItem);
                }
                catch (AutoCursorLockException ex)
                {
                    failures.Add($"Failed to create process list item for path '{appLock.Path}': {ex.Message}");
                }
            }
        }

        var userSettings = new UserSettings(
            processes,
            userSettingsModel.HotKey?.ToViewModel(id: 0)
        );

        return new ConversionResult<UserSettings>(userSettings, failures);
    }

    /// <summary>
    /// Converts a <see cref="UserSettings"/> to a <see cref="UserSettingsModel"/>.
    /// </summary>
    /// <param name="userSettings">The view model.</param>
    /// <returns>The model.</returns>
    public static UserSettingsModel ToModel(this UserSettings userSettings)
    {
        return new UserSettingsModel
        {
            HotKey = userSettings.HotKey?.ToModel(),
            AppLocks = userSettings.EnabledProcesses.Select(p => p.ToModel()).ToArray(),
        };
    }
}

/// <summary>
/// Represents the result of a conversion operation.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
/// <param name="Value">The resulting value.</param>
/// <param name="Failures">Any failures that were encountered during the conversion.</param>
public record ConversionResult<T>(T Value, IReadOnlyList<string> Failures);
