namespace AutoCursorLock.App;

using System;
using System.IO;

public static class Paths
{
    public static string AppDataPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AutoCursorLock");

    public static string UserSettingsPath => Path.Combine(AppDataPath, "settings.json");

    public static string LogPath => Path.Combine(AppDataPath, "log.txt");
}