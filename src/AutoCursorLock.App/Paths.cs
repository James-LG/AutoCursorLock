// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App;

using System;
using System.IO;

/// <summary>
/// Paths used by the application.
/// </summary>
public static class Paths
{
    /// <summary>
    /// Gets the location of this application's data folder.
    /// </summary>
    public static string AppDataPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AutoCursorLock");

    /// <summary>
    /// Gets the location of the user settings file.
    /// </summary>
    public static string UserSettingsPath => Path.Combine(AppDataPath, "settings.json");

    /// <summary>
    /// Gets the location of the log file.
    /// </summary>
    public static string LogPath => Path.Combine(AppDataPath, "log.txt");
}
