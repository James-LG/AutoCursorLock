using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace AutoCursorLock
{
    public record ProcessListItem
    {
        public ProcessListItem(string name, string path)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Path = path ?? throw new ArgumentNullException(nameof(path));

            using var ico = System.Drawing.Icon.ExtractAssociatedIcon(path);
            Icon = Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        public string Name { get; }

        public string Path { get; }
         
        [JsonIgnore]
        public BitmapSource Icon { get; }
    }
}
