using CommunityToolkit.Maui.Converters;
using Neuron.Nexus.Models;
using System.Globalization;

namespace Neuron.Nexus.Converters
{
    public class PickerLocaleDisplayConverter : BaseConverterOneWay<LanguageOption, string>
    {
        public override string DefaultConvertReturnValue { get; set; } = string.Empty;

        public override string ConvertFrom(LanguageOption value, CultureInfo culture)
        {
            return $"{value.LanguageName} ({value.NativeLanguageName})";
        }
    }
}
