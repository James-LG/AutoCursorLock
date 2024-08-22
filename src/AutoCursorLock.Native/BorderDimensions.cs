// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Native;

public record BorderDimensions(int Top, int Bottom, int Left, int Right);

public static class BorderDimensionsExtensions
{
    internal static BorderDimensions FromNative(this NativeMethods.NativeRectangle rect)
    {
        return new BorderDimensions(rect.top, rect.bottom, rect.left, rect.right);
    }
}
