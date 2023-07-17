using CommunityToolkit.Maui.Converters;
using Neuron.Nexus.Models;
using System.Globalization;

namespace Neuron.Nexus.Converters
{
    public class PickerLocaleDisplayConverter : BaseConverterOneWay<Language, string>
    {
        public override string DefaultConvertReturnValue { get; set; } = string.Empty;

        public override string ConvertFrom(Language value, CultureInfo culture)
        {
            return $"{value.LanguageName} ({value.FullLanguageCode})";
        }
    }
}
