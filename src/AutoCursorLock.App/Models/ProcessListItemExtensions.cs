namespace AutoCursorLock.App.Models;

using AutoCursorLock.Models;
using AutoCursorLock.Sdk.Models;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

internal static class ProcessListItemExtensions
{
    public static ProcessListItem FromPath(string path)
    {
        using var icon = System.Drawing.Icon.ExtractAssociatedIcon(path);
        var bitmapIcon = icon is null
            ? null
            : Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        var process = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(path))
            .FirstOrDefault()
            ?? throw new AutoCursorLockException($"Could not find process for path: {path}");

        return new ProcessListItem(process.ProcessName, path, AppLockType.Window, bitmapIcon);
    }

    public static AppLockSettingsModel ToModel(this ProcessListItem processListItem)
    {
        return new AppLockSettingsModel(
            processListItem.Path,
            processListItem.AppLockType
        );
    }

    public static ProcessListItem ToViewModel(AppLockSettingsModel appLockSettingsModel)
    {
        var processListItem = FromPath(appLockSettingsModel.Path);
        return processListItem with
        {
            AppLockType = appLockSettingsModel.Type
        };
    }
}