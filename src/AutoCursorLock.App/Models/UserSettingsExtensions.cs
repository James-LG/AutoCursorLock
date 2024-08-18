namespace AutoCursorLock.App.Models;

using AutoCursorLock.Models;
using AutoCursorLock.Sdk.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

public static class UserSettingsExtensions
{
    public static ConversionResult<UserSettings> ToViewModel(this UserSettingsModel userSettingsModel)
    {
        var failures = new List<string>();
        var processes = new ObservableCollection<ProcessListItem>();
        foreach (var appLock in userSettingsModel.AppLocks)
        {
            if (File.Exists(appLock.Path))
            {
                try
                {
                    var processListItem = ProcessListItemExtensions.FromPath(appLock.Path);
                    processes.Add(processListItem);
                }
                catch (AutoCursorLockException ex)
                {
                    failures.Add($"Failed to create process list item for path '{appLock.Path}': {ex.Message}");
                }
            }
        }

        var userSettings = new UserSettings(
            processes,
            userSettingsModel.HotKey?.ToViewModel(id: 0)
        );

        return new ConversionResult<UserSettings>(userSettings, failures);
    }

    public static UserSettingsModel ToModel(this UserSettings userSettings)
    {
        return new UserSettingsModel
        {
            HotKey = userSettings.HotKey?.ToModel(),
            AppLocks = userSettings.EnabledProcesses.Select(p => p.ToModel()).ToArray()
        };
    }
}

public record ConversionResult<T>(T Value, IReadOnlyList<string> Failures);
