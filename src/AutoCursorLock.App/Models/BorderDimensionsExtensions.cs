// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App.Models;

using AutoCursorLock.Native;
using AutoCursorLock.Sdk.Models;

/// <summary>
/// Extensions for <see cref="BorderDimensions"/>.
/// </summary>
internal static class BorderDimensionsExtensions
{
    /// <summary>
    /// Applies a margin to the border dimensions.
    /// </summary>
    /// <param name="dimensions">The dimensions to adjust.</param>
    /// <param name="margin">The margin to apply.</param>
    /// <returns>The adjusted dimensions.</returns>
    public static BorderDimensions ApplyMargin(this BorderDimensions dimensions, AppLockMargin margin)
    {
        return new BorderDimensions(
            dimensions.Top + margin.Top,
            dimensions.Bottom - margin.Bottom,
            dimensions.Left + margin.Left,
            dimensions.Right - margin.Right
        );
    }
}
