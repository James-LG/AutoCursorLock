// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Native;

/// <summary>
/// Handles application specific tasks.
/// </summary>
public static class ApplicationHandler
{
    /// <summary>
    /// Gets the size in pixel of a window's border.
    /// </summary>
    /// <param name="window">The handle of the window.</param>
    /// <returns>Returns the border size in pixel.</returns>
    public static BorderDimensions GetWindowBorderSizes(IntPtr window)
    {
        var windowBorderSizes = default(NativeMethods.NativeRectangle);

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

        return new BorderDimensions(windowBorderSizes.top, windowBorderSizes.bottom, windowBorderSizes.left, windowBorderSizes.right);
    }

    /// <summary>
    /// Gets the border dimensions of the application.
    /// </summary>
    /// <param name="window">The window handle.</param>
    /// <returns>The border dimensions of the window.</returns>
    /// <exception cref="InvalidOperationException">If the native methods could not get the window's rectangle.</exception>
    public static BorderDimensions GetApplicationBorders(IntPtr window)
    {
        if (NativeMethods.GetWindowRect(window, out var rect))
        {
            var border = ApplicationHandler.GetWindowBorderSizes(window);

            rect.top += border.Top;
            rect.bottom -= border.Bottom;
            rect.left += border.Left;
            rect.right -= border.Right;

            return new BorderDimensions(rect.top, rect.bottom, rect.left, rect.right);
        }

        throw new InvalidOperationException("Failed to get window rect.");
    }

    /// <summary>
    /// Gets the border dimensions of the monitor the window is on.
    /// </summary>
    /// <param name="window">The window handle.</param>
    /// <returns>The border dimensions of the monitor the window is on.</returns>
    public static BorderDimensions GetMonitorBorders(IntPtr window)
    {
        var monitor = NativeMethods.MonitorFromWindow(window, NativeMethods.MONITOR_DEFAULTTONEAREST);

        var monitorInfo = new NativeMethods.NativeMonitorInfo();
        NativeMethods.GetMonitorInfo(monitor, monitorInfo);

        return BorderDimensionsExtensions.FromNative(monitorInfo.Monitor);
    }
}
