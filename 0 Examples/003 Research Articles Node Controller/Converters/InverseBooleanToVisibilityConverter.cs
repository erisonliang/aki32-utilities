using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Aki32Utilities.UsageExamples.ResearchArticlesNodeController.Converters;
public class BooleanToForegroundBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var boolValue = (bool)value;
        return boolValue ? Brushes.Black : Brushes.DarkGray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            var brush = (SolidColorBrush)value;
            return brush.Color == Brushes.Black.Color;
        }
        catch (Exception)
        {
            return true;
        }
    }
}
