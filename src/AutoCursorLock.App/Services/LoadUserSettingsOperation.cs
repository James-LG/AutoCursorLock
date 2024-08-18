namespace AutoCursorLock.App.Services;
using AutoCursorLock.Sdk.Models;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public class LoadUserSettingsOperation(
    ILogger<LoadUserSettingsOperation> logger
)
{
    public async Task<UserSettingsModel> InvokeAsync()
    {
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