// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App.Services;

using AutoCursorLock.Sdk.Models;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

/// <summary>
/// Operation for saving user settings.
/// </summary>
public class SaveUserSettingsOperation
{
    /// <summary>
    /// Saves the user settings to the file.
    /// </summary>
    /// <param name="userSettingsModel">The user settings to save.</param>
    /// <returns>Task.</returns>
    public async Task InvokeAsync(UserSettingsModel userSettingsModel)
    {
        var json = JsonSerializer.Serialize(userSettingsModel);
        await File.WriteAllTextAsync(Paths.UserSettingsPath, json);
    }
}
