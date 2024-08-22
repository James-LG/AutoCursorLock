// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App.Extensions;

using AutoCursorLock.Native;
using AutoCursorLock.Sdk.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

/// <summary>
/// Converts a <see cref="HotKey"/> to a string.
/// </summary>
public class HotKeyToStringConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is HotKey hotKey)
        {
            var keyString = KeyInterop.KeyFromVirtualKey(hotKey.Key).ToString();
            var modifierKeys = hotKey.ModifierKeys.Where(x => x != ModifierKey.None).ToArray();
            if (modifierKeys.Length == 0)
            {
                return keyString;
            }

            var modifierString = string.Join("+", modifierKeys);
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
