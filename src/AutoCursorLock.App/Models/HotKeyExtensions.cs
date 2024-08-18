namespace AutoCursorLock.App.Models;

using AutoCursorLock.Native;
using AutoCursorLock.Sdk.Models;

public static class HotKeyExtensions
{
    public static HotKey ToViewModel(this HotKeyModel hotKey, int id)
    {
        return new HotKey(
            id,
            hotKey.Modifiers,
            hotKey.VirtualKey
        );
    }

    public static HotKeyModel ToModel(this HotKey hotKey)
    {
        return new HotKeyModel
        {
            Modifiers = hotKey.ModifierKeys,
            VirtualKey = hotKey.Key
        };
    }
}
