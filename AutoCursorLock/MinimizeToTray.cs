// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Forms;

    /// <summary>
    /// Class implementing support for "minimize to tray" functionality.
    /// </summary>
    public static class MinimizeToTray
    {
        /// <summary>
        /// Enables "minimize to tray" behavior for the specified Window.
        /// </summary>
        /// <param name="window">Window to enable the behavior for.</param>
        public static void Enable(Window window)
        {
            // No need to track this instance; its event handlers will keep it alive
            new MinimizeToTrayInstance(window);
        }

        /// <summary>
        /// Class implementing "minimize to tray" functionality for a Window instance.
        /// </summary>
        private class MinimizeToTrayInstance
        {
            private Window window;
            private NotifyIcon? notifyIcon;
            private bool balloonShown;

            /// <summary>
            /// Initializes a new instance of the <see cref="MinimizeToTrayInstance"/> class.
            /// </summary>
            /// <param name="window">Window instance to attach to.</param>
            public MinimizeToTrayInstance(Window window)
            {
                Debug.Assert(window != null, "window parameter is null.");
                this.window = window;
                this.window.StateChanged += new EventHandler(HandleStateChanged);
            }

            /// <summary>
            /// Handles the Window's StateChanged event.
            /// </summary>
            /// <param name="sender">Event source.</param>
            /// <param name="e">Event arguments.</param>
            private void HandleStateChanged(object? sender, EventArgs e)
            {
                if (this.notifyIcon == null)
                {
                    var icon = Assembly.GetEntryAssembly()?.Location;

                    // Initialize NotifyIcon instance "on demand"
                    this.notifyIcon = new NotifyIcon();

                    if (icon != null)
                    {
                        this.notifyIcon.Icon = Icon.ExtractAssociatedIcon(icon);
                    }

                    this.notifyIcon.MouseClick += new MouseEventHandler(HandleNotifyIconOrBalloonClicked);
                    this.notifyIcon.BalloonTipClicked += new EventHandler(HandleNotifyIconOrBalloonClicked);
                }

                // Update copy of Window Title in case it has changed
                this.notifyIcon.Text = this.window.Title;

                // Show/hide Window and NotifyIcon
                var minimized = this.window.WindowState == WindowState.Minimized;
                this.window.ShowInTaskbar = !minimized;
                this.notifyIcon.Visible = minimized;
                if (minimized && !this.balloonShown)
                {
                    // If this is the first time minimizing to the tray, show the user what happened
                    this.notifyIcon.ShowBalloonTip(1000, null, this.window.Title, ToolTipIcon.None);
                    this.balloonShown = true;
                }
            }

            /// <summary>
            /// Handles a click on the notify icon or its balloon.
            /// </summary>
            /// <param name="sender">Event source.</param>
            /// <param name="e">Event arguments.</param>
            private void HandleNotifyIconOrBalloonClicked(object? sender, EventArgs e)
            {
                // Restore the Window
                this.window.WindowState = WindowState.Normal;
            }
        }
    }
}
