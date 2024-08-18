// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App.Extensions;

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

/// <summary>
/// Converts a keyboard key to its string value.
/// </summary>
public class KeyToStringConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Key key)
        {
            var text = key.ToString();
            return text;
        }

        return Binding.DoNothing;
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
