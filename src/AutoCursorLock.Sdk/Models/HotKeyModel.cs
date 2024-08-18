namespace AutoCursorLock.Sdk.Models;

using System.Text.Json.Serialization;

public record HotKeyModel
{
    [JsonRequired]
    required public ModifierKey[] Modifiers { get; init; }

    [JsonRequired]
    required public int VirtualKey { get; init; }
}
