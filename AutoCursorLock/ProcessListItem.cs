using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AutoCursorLock
{
    public record ProcessListItem
    {
        public ProcessListItem(string name, Uri path, BitmapSource icon)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Icon = icon ?? throw new ArgumentNullException(nameof(icon));
        }

        public string Name { get; }

        public Uri Path { get; }
         
        public BitmapSource Icon { get; }
    }
}
