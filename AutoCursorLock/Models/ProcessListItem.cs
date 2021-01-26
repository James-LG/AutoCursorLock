// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Models
{
    using System;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media.Imaging;
    using Newtonsoft.Json;

    public class ProcessListItem
    {
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

        public string Name { get; }

        public string Path { get; }

        [JsonIgnore]
        public BitmapSource? Icon { get; }
    }
}
