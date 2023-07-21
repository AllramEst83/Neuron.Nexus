using Neuron.Nexus.Models;
using System.Globalization;

namespace Neuron.Nexus.Converters
{
    public class UserTemplateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is UserMessage userMessage)
            {
                
                switch (userMessage.User)
                {
                    case 1:
                        var hasvalue1 = Application.Current.Resources.TryGetValue("chatColorsUserOne", out var userOneColor);

                        return hasvalue1 ? (Color)userOneColor : Colors.Wheat;
                    case 2:
                        var hasvalue2 = Application.Current.Resources.TryGetValue("chatColorsUserTwo", out var userTwocolor);

                        return hasvalue2 ? (Color)userTwocolor : Colors.Wheat;
                    default:
                        return Colors.Wheat;
                }
            }

            return Colors.Wheat;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
