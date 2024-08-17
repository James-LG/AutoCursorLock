// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Native;

public record WindowBorderSize(int Top, int Bottom, int Left, int Right);

public static class WindowBorderSizeExtensions
{
    internal static WindowBorderSize FromNative(this NativeMethods.RECT rect)
    {
        return new WindowBorderSize(rect.top, rect.bottom, rect.left, rect.right);
    }
}
