using Neuron.Nexus.Models;
using System.Globalization;

namespace Neuron.Nexus.Converters;

public class UserToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var user = (int)value;
        return user == 1 ? Color.FromArgb("#4287f5") : Color.FromArgb("#4287f5");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

