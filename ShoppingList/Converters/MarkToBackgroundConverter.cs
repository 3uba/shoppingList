using System.Globalization;

namespace ShoppingList.Converters;

public class MarkToBackgroundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool mark = (bool)value;
        return mark ? Color.FromRgb(30,30,30) : Color.FromRgba(0,0,0,0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? 1 : 0;
    }
}