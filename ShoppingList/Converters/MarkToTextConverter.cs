using System.Globalization;

namespace ShoppingList.Converters;

public class MarkToTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool mark = (bool)value;
        return mark ? "Odznacz" : "Zaznacz";
    }
    
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? 1 : 0;
    }
}