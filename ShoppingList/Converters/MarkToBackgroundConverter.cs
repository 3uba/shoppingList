using System.Globalization;

namespace ShoppingList.Converters;

public class MarkToBackgroundConverter : IMarkupExtension, IValueConverter
{
    public object NullValue { get; set; }
    public object NotNullValue { get; set; }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool mark && mark)
        {
            return new SolidColorBrush(Colors.Grey);
        }

        return new SolidColorBrush(Colors.Transparent);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}