// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Dtos
{
    /// <summary>
    /// Represents a process to be displayed in the process selection list.
    /// </summary>
    public class ProcessListItemDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessListItemDto"/> class.
        /// </summary>
        /// <param name="name">The name of the process.</param>
        /// <param name="path">The path of the process' executable.</param>
        public ProcessListItemDto(string name, string path)
        {
            Name = name;
            Path = path;
        }

        /// <summary>
        /// Gets the name of the process.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the path of the process' executable.
        /// </summary>
        public string Path { get; init; }
    }
}
