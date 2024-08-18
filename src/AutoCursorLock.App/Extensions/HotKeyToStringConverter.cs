namespace AutoCursorLock.App.Extensions;

using AutoCursorLock.Native;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

public class HotKeyToStringConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is HotKey hotKey)
        {
            var keyString = KeyInterop.KeyFromVirtualKey(hotKey.Key).ToString();
            if (hotKey.ModifierKeys.Length == 0)
            {
                return keyString;
            }

            var modifierString = string.Join("+", hotKey.ModifierKeys);
            return $"{modifierString}+{keyString}";
        }

        return Binding.DoNothing;
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
