﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Aki32Utilities.WPFAppUtilities.NodeController.Converters;

public class InverseBooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var boolValue = (bool)value;
        return boolValue ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var visibility = (Visibility)value;
        return visibility == Visibility.Visible ? false : true;
    }
}
