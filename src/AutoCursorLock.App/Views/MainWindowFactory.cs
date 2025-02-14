// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App.Views;

using AutoCursorLock.App.Models;
using AutoCursorLock.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.Threading.Tasks;

/// <summary>
/// Factory for creating the main window.
/// </summary>
internal class MainWindowFactory(
    LoadUserSettingsOperation loadUserSettingsOperation,
    LoggingLevelSwitch loggingLevelSwitch,
    ILogger<MainWindowFactory> logger,
    IServiceProvider serviceProvider)
{
    /// <summary>
    /// Creates the main window.
    /// </summary>
    /// <returns>The main window.</returns>
    public async Task<MainWindow> CreateAsync()
    {
        var userSettingsModel = await loadUserSettingsOperation.InvokeAsync();
        var conversionResult = userSettingsModel.ToViewModel(loggingLevelSwitch);

        foreach (var failure in conversionResult.Failures)
        {
            logger.LogError("Failure creating user settings view model: {FAILURE}", failure);
        }

        return ActivatorUtilities.CreateInstance<MainWindow>(serviceProvider, conversionResult.Value);
    }
}
