// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Dtos
{
    using AutoCursorLock.Models;
    using AutoCursorLock.Native;

    /// <summary>
    /// Represents a process to be displayed in the process selection list.
    /// </summary>
    public class UserSettingsDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSettingsDto"/> class.
        /// </summary>
        /// <param name="enabledProcesses">The processes enabled by the user.</param>
        /// <param name="hotKey">The hot key to toggle the cursor lock.</param>
        public UserSettingsDto(ProcessListItemDto[] enabledProcesses, HotKey hotKey)
        {
            EnabledProcesses = enabledProcesses;
            HotKey = hotKey;
        }

        /// <summary>
        /// Gets the processes that the user has selected to lock the cursor on when in focus.
        /// </summary>
        /// <remarks>
        /// When a window from an enabled process is focused by the user, this program will lock the cursor
        /// to the bounds of the window until focus is changed to another process.
        /// </remarks>
        public ProcessListItemDto[] EnabledProcesses { get; init; }

        /// <summary>
        /// Gets the hot key used to toggle the programs locking feature.
        /// </summary>
        public HotKey HotKey { get; init; }
    }
}
