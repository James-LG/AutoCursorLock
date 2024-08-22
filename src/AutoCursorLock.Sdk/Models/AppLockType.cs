// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Sdk.Models;

/// <summary>
/// Represents the type of app lock to use.
/// </summary>
public enum AppLockType
{
    /// <summary>
    /// Lock the cursor to the application's window.
    /// </summary>
    Window,

    /// <summary>
    /// Lock the cursor to the application's monitor.
    /// </summary>
    Monitor,
}
