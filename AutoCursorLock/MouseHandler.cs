// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock
{
    using System;

    internal static class MouseHandler
    {
        public static bool LockCursor(IntPtr hwnd)
        {
            if (NativeMethods.GetWindowRect(hwnd, out var rect))
            {
                var border = GetWindowBorderSizes(hwnd);

                rect.top += border.top;
                rect.bottom -= border.bottom;
                rect.left += border.left;
                rect.right -= border.right;

                return NativeMethods.ClipCursor(rect);
            }

            return false;
        }

        public static bool UnlockCursor()
        {
            return NativeMethods.ClipCursor(IntPtr.Zero);
        }

        /// <summary>
        /// Gets the size in pixel of a window's border.
        /// </summary>
        /// <param name="window">The handle of the window.</param>
        /// <returns>Returns the border size in pixel.</returns>
        public static NativeMethods.RECT GetWindowBorderSizes(IntPtr window)
        {
            var windowBorderSizes = default(NativeMethods.RECT);

            var styles = NativeMethods.GetWindowLong(window, NativeMethods.GetWindowLongIndex.GWL_STYLE);

            // Window has title-bar
            if (styles.HasFlag(NativeMethods.WindowStyles.WS_CAPTION))
            {
                windowBorderSizes.top += NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_CYCAPTION);
            }

            // Window has re-sizable borders
            if (styles.HasFlag(NativeMethods.WindowStyles.WS_THICKFRAME))
            {
                windowBorderSizes.left += NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_CXSIZEFRAME);
                windowBorderSizes.right += NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_CXSIZEFRAME);
                windowBorderSizes.top += NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_CYSIZEFRAME);
                windowBorderSizes.bottom += NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_CYSIZEFRAME);
            }
            else if (styles.HasFlag(NativeMethods.WindowStyles.WS_BORDER) || styles.HasFlag(NativeMethods.WindowStyles.WS_CAPTION))
            {
                // Window has normal borders
                windowBorderSizes.left += NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_CXFIXEDFRAME);
                windowBorderSizes.right += NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_CXFIXEDFRAME);
                windowBorderSizes.top += NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_CYFIXEDFRAME);
                windowBorderSizes.bottom += NativeMethods.GetSystemMetrics(NativeMethods.SystemMetric.SM_CYFIXEDFRAME);
            }

            return windowBorderSizes;
        }
    }
}
