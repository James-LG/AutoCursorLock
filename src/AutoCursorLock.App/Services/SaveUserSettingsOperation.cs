namespace AutoCursorLock.App.Services;

using AutoCursorLock.Models;
using AutoCursorLock.Sdk.Models;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public class SaveUserSettingsOperation(
)
{
    public async Task InvokeAsync(UserSettingsModel userSettingsModel)
    {
        var json = JsonSerializer.Serialize(userSettingsModel);
        await File.WriteAllTextAsync(Paths.UserSettingsPath, json);
    }
}
