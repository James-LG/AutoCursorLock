namespace AutoCursorLock.App.Views;

using AutoCursorLock.App.Models;
using AutoCursorLock.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class MainWindowFactory(
    LoadUserSettingsOperation loadUserSettingsOperation,
    ILogger<MainWindowFactory> logger,
    IServiceProvider serviceProvider)
{
    public async Task<MainWindow> CreateAsync()
    {
        var userSettingsModel = await loadUserSettingsOperation.InvokeAsync();
        var conversionResult = userSettingsModel.ToViewModel();

        foreach (var failure in conversionResult.Failures)
        {
            logger.LogError("Failure creating user settings view model: {FAILURE}", failure);
        }

        return ActivatorUtilities.CreateInstance<MainWindow>(serviceProvider, conversionResult.Value);
    }
}
