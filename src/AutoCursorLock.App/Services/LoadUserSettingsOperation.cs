// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App.Services;

using AutoCursorLock.Sdk.Models;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

/// <summary>
/// Operation for loading user settings.
/// </summary>
public class LoadUserSettingsOperation(
    ILogger<LoadUserSettingsOperation> logger
)
{
    /// <summary>
    /// Loads user settings from the file.
    /// </summary>
    /// <returns>The user settings.</returns>
    public async Task<UserSettingsModel> InvokeAsync()
    {
        if (!File.Exists(Paths.UserSettingsPath))
        {
            logger.LogDebug("User settings file does not exist, returning default settings");
            return new UserSettingsModel
            {
                HotKey = default,
                AppLocks = [],
            };
        }

        var fileContent = await File.ReadAllTextAsync(Paths.UserSettingsPath);

        UserSettingsModel? userSettingsModel;

        try
        {
            userSettingsModel = JsonSerializer.Deserialize<UserSettingsModel>(fileContent);
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Failed to deserialize user settings file");
            userSettingsModel = null;
        }

        if (userSettingsModel is null)
        {
            return new UserSettingsModel
            {
                HotKey = default,
                AppLocks = [],
            };
        }

        return userSettingsModel;
    }
}
