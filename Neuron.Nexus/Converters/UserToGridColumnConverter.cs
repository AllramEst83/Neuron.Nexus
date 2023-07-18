using Neuron.Nexus.Models;
using System.Globalization;

namespace Neuron.Nexus.Converters;

public class UserToGridColumnConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var user = (int)value;
        return user == 1 ? 1 : 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

