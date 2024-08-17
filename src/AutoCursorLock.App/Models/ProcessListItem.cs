// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Models
{
    using System;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media.Imaging;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a process to be displayed in the process selection list.
    /// </summary>
    public class ProcessListItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessListItem"/> class.
        /// </summary>
        /// <param name="name">The name of the process.</param>
        /// <param name="path">The path of the proceess' executable.</param>
        public ProcessListItem(string name, string path)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Path = path ?? throw new ArgumentNullException(nameof(path));

            using var ico = System.Drawing.Icon.ExtractAssociatedIcon(path);
            if (ico != null)
            {
                Icon = Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
        }

        /// <summary>
        /// Gets the name of the process.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the path of the process' executable.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Gets the icon image of the process.
        /// </summary>
        [JsonIgnore]
        public BitmapSource? Icon { get; }
    }
}
