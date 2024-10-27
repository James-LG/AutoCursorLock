// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Sdk.Models;

/// <summary>
/// Represents the margin around a window for an app lock.
/// </summary>
/// <param name="Left">The left-side margin.</param>
/// <param name="Top">The top-side margin.</param>
/// <param name="Right">The right-side margin.</param>
/// <param name="Bottom">THe bottom-side margin.</param>
public record AppLockMargin(
    int Left,
    int Top,
    int Right,
    int Bottom
);
