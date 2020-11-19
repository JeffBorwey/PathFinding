﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Wpf3dVersion.Converters
{
    internal class MaterialOpacityValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Math.Round((double)value, 2);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Math.Round((double)value, 2);
        }
    }
}
