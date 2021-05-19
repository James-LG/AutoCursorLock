// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.Extensions
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Converts a boolean value to one of two colors.
    /// </summary>
    public class BoolColorSelector : IValueConverter
    {
        private static readonly Color TrueColor = Colors.Green;
        private static readonly Color FalseColor = Colors.Red;

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                bool => (bool)value ? new SolidColorBrush(TrueColor) : new SolidColorBrush(FalseColor),
                _ => Binding.DoNothing
            };
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
