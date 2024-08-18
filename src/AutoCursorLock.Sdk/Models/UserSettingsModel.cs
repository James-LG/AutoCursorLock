namespace AutoCursorLock.Sdk.Models;

using System.Text.Json.Serialization;

public record UserSettingsModel
{
    public HotKeyModel? HotKey { get; init; }

    [JsonRequired]
    required public AppLockSettingsModel[] AppLocks { get; init; }
}