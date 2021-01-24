// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock
{
    using System;

    /// <summary>
    /// Event arguments containing application data.
    /// </summary>
    internal class ApplicationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationEventArgs"/> class.
        /// </summary>
        /// <param name="handle">The application's window handle.</param>
        /// <param name="processId">The application's process ID.</param>
        /// <param name="processName">The application's process name.</param>
        public ApplicationEventArgs(IntPtr handle, uint processId, string processName)
        {
            Handle = handle;
            ProcessId = processId;
            ProcessName = processName ?? throw new ArgumentNullException(nameof(processName));
        }

        /// <summary>
        /// Gets the application's window handle.
        /// </summary>
        public IntPtr Handle { get; }

        /// <summary>
        /// Gets the application's process ID.
        /// </summary>
        public uint ProcessId { get; }

        /// <summary>
        /// Gets the application's process name.
        /// </summary>
        public string ProcessName { get; }
    }
}
