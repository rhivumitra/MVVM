using System.Globalization;
using System.Windows;
using System.Windows.Data;

// ReSharper disable NullnessAnnotationConflictWithJetBrainsAnnotations

namespace [Redact];

public class BooleanInverterConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType == typeof(Visibility))
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        if (value is bool booleanValue)
        {
            return !booleanValue;
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
