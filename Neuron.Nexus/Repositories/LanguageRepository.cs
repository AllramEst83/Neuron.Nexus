using Neuron.Nexus.Models;

namespace Neuron.Nexus.Repositories
{
    public interface ILanguageRepository
    {
        List<LanguageOption> GetLanguagesOrderByLanguageName();
        List<CultureOption> GetCultureOptions();
    }
    public class LanguageRepository : ILanguageRepository
    {
        public LanguageRepository()
        {
        }

        public List<CultureOption> GetCultureOptions()
        {
            return new List<CultureOption>()
            {
                new CultureOption(){ Id = 1, CultureCode = "sv-SE", CultureDisplayName = "Svenska", CultureIconName = "swedenflag" },
                new CultureOption(){ Id = 2, CultureCode = "en-US", CultureDisplayName = "English", CultureIconName = "usflag"}

            }.OrderBy(x => x.CultureDisplayName).ToList();
        }

        /// <summary>
        /// Compatible langugages
        /// https://learn.microsoft.com/en-us/azure/ai-services/speech-service/language-support?tabs=stt
        /// </summary>
        /// <returns></returns>
        public List<LanguageOption> GetLanguagesOrderByLanguageName()
        {
            return new List<LanguageOption>
            {
                new LanguageOption() { Id = 1, FullLanguageCode = "af-ZA", ShortLanguageCode = "af", LanguageName = "Afrikaans (South Africa)", NativeLanguageName = "Afrikaans" },
                new LanguageOption() { Id = 2, FullLanguageCode = "am-ET", ShortLanguageCode = "am", LanguageName = "Amharic (Ethiopia)", NativeLanguageName = "አማርኛ" },
                new LanguageOption() { Id = 3, FullLanguageCode = "ar-AE", ShortLanguageCode = "ar", LanguageName = "Arabic (United Arab Emirates)", NativeLanguageName = "العربية (الإمارات العربية المتحدة)" },
                new LanguageOption() { Id = 4, FullLanguageCode = "ar-BH", ShortLanguageCode = "ar", LanguageName = "Arabic (Bahrain)", NativeLanguageName = "العربية (البحرين)" },
                new LanguageOption() { Id = 5, FullLanguageCode = "ar-DZ", ShortLanguageCode = "ar", LanguageName = "Arabic (Algeria)", NativeLanguageName = "العربية (الجزائر)" },
                new LanguageOption() { Id = 6, FullLanguageCode = "ar-EG", ShortLanguageCode = "ar", LanguageName = "Arabic (Egypt)", NativeLanguageName = "العربية (مصر)" },
                new LanguageOption() { Id = 7, FullLanguageCode = "ar-IL", ShortLanguageCode = "ar", LanguageName = "Arabic (Israel)", NativeLanguageName = "العربية (إسرائيل)" },
                new LanguageOption() { Id = 8, FullLanguageCode = "ar-IQ", ShortLanguageCode = "ar", LanguageName = "Arabic (Iraq)", NativeLanguageName = "العربية (العراق)" },
                new LanguageOption() { Id = 9, FullLanguageCode = "ar-JO", ShortLanguageCode = "ar", LanguageName = "Arabic (Jordan)", NativeLanguageName = "العربية (الأردن)" },
                new LanguageOption() { Id = 10, FullLanguageCode = "ar-KW", ShortLanguageCode = "ar", LanguageName = "Arabic (Kuwait)", NativeLanguageName = "العربية (الكويت)" },
                new LanguageOption() { Id = 11, FullLanguageCode = "ar-LB", ShortLanguageCode = "ar", LanguageName = "Arabic (Lebanon)", NativeLanguageName = "العربية (لبنان)" },
                new LanguageOption() { Id = 12, FullLanguageCode = "ar-LY", ShortLanguageCode = "ar", LanguageName = "Arabic (Libya)", NativeLanguageName = "العربية (ليبيا)" },
                new LanguageOption() { Id = 13, FullLanguageCode = "ar-MA", ShortLanguageCode = "ar", LanguageName = "Arabic (Morocco)", NativeLanguageName = "العربية (المغرب)" },
                new LanguageOption() { Id = 14, FullLanguageCode = "ar-OM", ShortLanguageCode = "ar", LanguageName = "Arabic (Oman)", NativeLanguageName = "العربية (عُمان)" },
                new LanguageOption() { Id = 15, FullLanguageCode = "ar-PS", ShortLanguageCode = "ar", LanguageName = "Arabic (Palestinian Authority)", NativeLanguageName = "العربية (السلطة الفلسطينية)" },
                new LanguageOption() { Id = 16, FullLanguageCode = "ar-QA", ShortLanguageCode = "ar", LanguageName = "Arabic (Qatar)", NativeLanguageName = "العربية (قطر)" },
                new LanguageOption() { Id = 17, FullLanguageCode = "ar-SA", ShortLanguageCode = "ar", LanguageName = "Arabic (Saudi Arabia)", NativeLanguageName = "العربية (المملكة العربية السعودية)" },
                new LanguageOption() { Id = 18, FullLanguageCode = "ar-SY", ShortLanguageCode = "ar", LanguageName = "Arabic (Syria)", NativeLanguageName = "العربية (سوريا)" },
                new LanguageOption() { Id = 19, FullLanguageCode = "ar-TN", ShortLanguageCode = "ar", LanguageName = "Arabic (Tunisia)", NativeLanguageName = "العربية (تونس)" },
                new LanguageOption() { Id = 20, FullLanguageCode = "ar-YE", ShortLanguageCode = "ar", LanguageName = "Arabic (Yemen)", NativeLanguageName = "العربية (اليمن)" },
                new LanguageOption() { Id = 21, FullLanguageCode = "az-AZ", ShortLanguageCode = "az", LanguageName = "Azerbaijani (Latin, Azerbaijan)", NativeLanguageName = "azərbaycan dili (Azərbaycan)" },
                new LanguageOption() { Id = 22, FullLanguageCode = "bg-BG", ShortLanguageCode = "bg", LanguageName = "Bulgarian (Bulgaria)", NativeLanguageName = "български (България)" },
                new LanguageOption() { Id = 23, FullLanguageCode = "bn-IN", ShortLanguageCode = "bn", LanguageName = "Bengali (India)", NativeLanguageName = "বাংলা (ভারত)" },
                new LanguageOption() { Id = 24, FullLanguageCode = "bs-BA", ShortLanguageCode = "bs", LanguageName = "Bosnian (Bosnia and Herzegovina)", NativeLanguageName = "Bosanski (Bosna i Hercegovina)" },
                new LanguageOption() { Id = 25, FullLanguageCode = "ca-ES", ShortLanguageCode = "ca", LanguageName = "Catalan", NativeLanguageName = "Català" },
                new LanguageOption() { Id = 26, FullLanguageCode = "cs-CZ", ShortLanguageCode = "cs", LanguageName = "Czech (Czechia)", NativeLanguageName = "Čeština (Česká republika)" },
                new LanguageOption() { Id = 27, FullLanguageCode = "cy-GB", ShortLanguageCode = "cy", LanguageName = "Welsh (United Kingdom)", NativeLanguageName = "Cymraeg (y Deyrnas Unedig)" },
                new LanguageOption() { Id = 28, FullLanguageCode = "da-DK", ShortLanguageCode = "da", LanguageName = "Danish (Denmark)", NativeLanguageName = "Dansk (Danmark)" },
                new LanguageOption() { Id = 29, FullLanguageCode = "de-AT", ShortLanguageCode = "de", LanguageName = "German (Austria)", NativeLanguageName = "Deutsch (Österreich)" },
                new LanguageOption() { Id = 30, FullLanguageCode = "de-CH", ShortLanguageCode = "de", LanguageName = "German (Switzerland)", NativeLanguageName = "Deutsch (Schweiz)" },
                new LanguageOption() { Id = 31, FullLanguageCode = "de-DE", ShortLanguageCode = "de", LanguageName = "German (Germany)", NativeLanguageName = "Deutsch (Deutschland)" },
                new LanguageOption() { Id = 32, FullLanguageCode = "el-GR", ShortLanguageCode = "el", LanguageName = "Greek (Greece)", NativeLanguageName = "Ελληνικά (Ελλάδα)" },
                new LanguageOption() { Id = 33, FullLanguageCode = "en-AU", ShortLanguageCode = "en", LanguageName = "English (Australia)", NativeLanguageName = "English (Australia)" },
                new LanguageOption() { Id = 34, FullLanguageCode = "en-CA", ShortLanguageCode = "en", LanguageName = "English (Canada)", NativeLanguageName = "English (Canada)" },
                new LanguageOption() { Id = 35, FullLanguageCode = "en-GB", ShortLanguageCode = "en", LanguageName = "English (United Kingdom)", NativeLanguageName = "English (United Kingdom)" },
                new LanguageOption() { Id = 36, FullLanguageCode = "en-GH", ShortLanguageCode = "en", LanguageName = "English (Ghana)", NativeLanguageName = "English (Ghana)" },
                new LanguageOption() { Id = 37, FullLanguageCode = "en-HK", ShortLanguageCode = "en", LanguageName = "English (Hong Kong SAR)", NativeLanguageName = "English (Hong Kong SAR)" },
                new LanguageOption() { Id = 38, FullLanguageCode = "en-IE", ShortLanguageCode = "en", LanguageName = "English (Ireland)", NativeLanguageName = "English (Ireland)" },
                new LanguageOption() { Id = 39, FullLanguageCode = "en-IN", ShortLanguageCode = "en", LanguageName = "English (India)", NativeLanguageName = "English (India)" },
                new LanguageOption() { Id = 40, FullLanguageCode = "en-KE", ShortLanguageCode = "en", LanguageName = "English (Kenya)", NativeLanguageName = "English (Kenya)" },
                new LanguageOption() { Id = 41, FullLanguageCode = "en-NG", ShortLanguageCode = "en", LanguageName = "English (Nigeria)", NativeLanguageName = "English (Nigeria)" },
                new LanguageOption() { Id = 42, FullLanguageCode = "en-NZ", ShortLanguageCode = "en", LanguageName = "English (New Zealand)", NativeLanguageName = "English (New Zealand)" },
                new LanguageOption() { Id = 43, FullLanguageCode = "en-PH", ShortLanguageCode = "en", LanguageName = "English (Philippines)", NativeLanguageName = "English (Philippines)" },
                new LanguageOption() { Id = 44, FullLanguageCode = "en-SG", ShortLanguageCode = "en", LanguageName = "English (Singapore)", NativeLanguageName = "English (Singapore)" },
                new LanguageOption() { Id = 45, FullLanguageCode = "en-TZ", ShortLanguageCode = "en", LanguageName = "English (Tanzania)", NativeLanguageName = "English (Tanzania)" },
                new LanguageOption() { Id = 46, FullLanguageCode = "en-US", ShortLanguageCode = "en", LanguageName = "English (United States)", NativeLanguageName = "English (United States)" },
                new LanguageOption() { Id = 47, FullLanguageCode = "en-ZA", ShortLanguageCode = "en", LanguageName = "English (South Africa)", NativeLanguageName = "English (South Africa)" },
                new LanguageOption() { Id = 48, FullLanguageCode = "es-AR", ShortLanguageCode = "es", LanguageName = "Spanish (Argentina)", NativeLanguageName = "Español (Argentina)" },
                new LanguageOption() { Id = 49, FullLanguageCode = "es-BO", ShortLanguageCode = "es", LanguageName = "Spanish (Bolivia)", NativeLanguageName = "Español (Bolivia)" },
                new LanguageOption() { Id = 50, FullLanguageCode = "es-CL", ShortLanguageCode = "es", LanguageName = "Spanish (Chile)", NativeLanguageName = "Español (Chile)" },
                new LanguageOption() { Id = 51, FullLanguageCode = "es-CO", ShortLanguageCode = "es", LanguageName = "Spanish (Colombia)", NativeLanguageName = "Español (Colombia)" },
                new LanguageOption() { Id = 52, FullLanguageCode = "es-CR", ShortLanguageCode = "es", LanguageName = "Spanish (Costa Rica)", NativeLanguageName = "Español (Costa Rica)" },
                new LanguageOption() { Id = 53, FullLanguageCode = "es-CU", ShortLanguageCode = "es", LanguageName = "Spanish (Cuba)", NativeLanguageName = "Español (Cuba)" },
                new LanguageOption() { Id = 54, FullLanguageCode = "es-DO", ShortLanguageCode = "es", LanguageName = "Spanish (Dominican Republic)", NativeLanguageName = "Español (República Dominicana)" },
                new LanguageOption() { Id = 55, FullLanguageCode = "es-EC", ShortLanguageCode = "es", LanguageName = "Spanish (Ecuador)", NativeLanguageName = "Español (Ecuador)" },
                new LanguageOption() { Id = 56, FullLanguageCode = "es-ES", ShortLanguageCode = "es", LanguageName = "Spanish (Spain)", NativeLanguageName = "Español (España)" },
                new LanguageOption() { Id = 57, FullLanguageCode = "es-GQ", ShortLanguageCode = "es", LanguageName = "Spanish (Equatorial Guinea)", NativeLanguageName = "Español (Guinea Ecuatorial)" },
                new LanguageOption() { Id = 58, FullLanguageCode = "es-GT", ShortLanguageCode = "es", LanguageName = "Spanish (Guatemala)", NativeLanguageName = "Español (Guatemala)" },
                new LanguageOption() { Id = 59, FullLanguageCode = "es-HN", ShortLanguageCode = "es", LanguageName = "Spanish (Honduras)", NativeLanguageName = "Español (Honduras)" },
                new LanguageOption() { Id = 60, FullLanguageCode = "es-MX", ShortLanguageCode = "es", LanguageName = "Spanish (Mexico)", NativeLanguageName = "Español (México)" },
                new LanguageOption() { Id = 61, FullLanguageCode = "es-NI", ShortLanguageCode = "es", LanguageName = "Spanish (Nicaragua)", NativeLanguageName = "Español (Nicaragua)" },
                new LanguageOption() { Id = 62, FullLanguageCode = "es-PA", ShortLanguageCode = "es", LanguageName = "Spanish (Panama)", NativeLanguageName = "Español (Panamá)" },
                new LanguageOption() { Id = 63, FullLanguageCode = "es-PE", ShortLanguageCode = "es", LanguageName = "Spanish (Peru)", NativeLanguageName = "Español (Perú)" },
                new LanguageOption() { Id = 64, FullLanguageCode = "es-PR", ShortLanguageCode = "es", LanguageName = "Spanish (Puerto Rico)", NativeLanguageName = "Español (Puerto Rico)" },
                new LanguageOption() { Id = 65, FullLanguageCode = "es-PY", ShortLanguageCode = "es", LanguageName = "Spanish (Paraguay)", NativeLanguageName = "Español (Paraguay)" },
                new LanguageOption() { Id = 66, FullLanguageCode = "es-SV", ShortLanguageCode = "es", LanguageName = "Spanish (El Salvador)", NativeLanguageName = "Español (El Salvador)" },
                new LanguageOption() { Id = 67, FullLanguageCode = "es-US", ShortLanguageCode = "es", LanguageName = "Spanish (United States)", NativeLanguageName = "Español (Estados Unidos)" },
                new LanguageOption() { Id = 68, FullLanguageCode = "es-UY", ShortLanguageCode = "es", LanguageName = "Spanish (Uruguay)", NativeLanguageName = "Español (Uruguay)" },
                new LanguageOption() { Id = 69, FullLanguageCode = "es-VE", ShortLanguageCode = "es", LanguageName = "Spanish (Venezuela)", NativeLanguageName = "Español (Venezuela)" },
                new LanguageOption() { Id = 70, FullLanguageCode = "et-EE", ShortLanguageCode = "et", LanguageName = "Estonian (Estonia)", NativeLanguageName = "eesti (Eesti)" },
                new LanguageOption() { Id = 71, FullLanguageCode = "eu-ES", ShortLanguageCode = "eu", LanguageName = "Basque", NativeLanguageName = "Euskara" },
                new LanguageOption() { Id = 72, FullLanguageCode = "fa-IR", ShortLanguageCode = "fa", LanguageName = "Persian (Iran)", NativeLanguageName = "فارسی (ایران)" },
                new LanguageOption() { Id = 73, FullLanguageCode = "fi-FI", ShortLanguageCode = "fi", LanguageName = "Finnish (Finland)", NativeLanguageName = "suomi (Suomi)" },
                new LanguageOption() { Id = 74, FullLanguageCode = "fil-PH", ShortLanguageCode = "fil", LanguageName = "Filipino (Philippines)", NativeLanguageName = "Filipino (Pilipinas)" },
                new LanguageOption() { Id = 75, FullLanguageCode = "fr-BE", ShortLanguageCode = "fr", LanguageName = "French (Belgium)", NativeLanguageName = "Français (Belgique)" },
                new LanguageOption() { Id = 76, FullLanguageCode = "fr-CA", ShortLanguageCode = "fr", LanguageName = "French (Canada)", NativeLanguageName = "Français (Canada)" },
                new LanguageOption() { Id = 77, FullLanguageCode = "fr-CH", ShortLanguageCode = "fr", LanguageName = "French (Switzerland)", NativeLanguageName = "Français (Suisse)" },
                new LanguageOption() { Id = 78, FullLanguageCode = "fr-FR", ShortLanguageCode = "fr", LanguageName = "French (France)", NativeLanguageName = "Français (France)" },
                new LanguageOption() { Id = 79, FullLanguageCode = "ga-IE", ShortLanguageCode = "ga", LanguageName = "Irish (Ireland)", NativeLanguageName = "Gaeilge (Éire)" },
                new LanguageOption() { Id = 80, FullLanguageCode = "gl-ES", ShortLanguageCode = "gl", LanguageName = "Galician", NativeLanguageName = "Galego" },
                new LanguageOption() { Id = 81, FullLanguageCode = "gu-IN", ShortLanguageCode = "gu", LanguageName = "Gujarati (India)", NativeLanguageName = "ગુજરાતી (ભારત)" },
                new LanguageOption() { Id = 82, FullLanguageCode = "he-IL", ShortLanguageCode = "he", LanguageName = "Hebrew (Israel)", NativeLanguageName = "עברית (ישראל)" },
                new LanguageOption() { Id = 83, FullLanguageCode = "hi-IN", ShortLanguageCode = "hi", LanguageName = "Hindi (India)", NativeLanguageName = "हिन्दी (भारत)" },
                new LanguageOption() { Id = 84, FullLanguageCode = "hr-HR", ShortLanguageCode = "hr", LanguageName = "Croatian (Croatia)", NativeLanguageName = "Hrvatski (Hrvatska)" },
                new LanguageOption() { Id = 85, FullLanguageCode = "hu-HU", ShortLanguageCode = "hu", LanguageName = "Hungarian (Hungary)", NativeLanguageName = "Magyar (Magyarország)" },
                new LanguageOption() { Id = 86, FullLanguageCode = "hy-AM", ShortLanguageCode = "hy", LanguageName = "Armenian (Armenia)", NativeLanguageName = "Հայերեն (Հայաստան)" },
                new LanguageOption() { Id = 87, FullLanguageCode = "id-ID", ShortLanguageCode = "id", LanguageName = "Indonesian (Indonesia)", NativeLanguageName = "Bahasa Indonesia (Indonesia)" },
                new LanguageOption() { Id = 88, FullLanguageCode = "is-IS", ShortLanguageCode = "is", LanguageName = "Icelandic (Iceland)", NativeLanguageName = "Íslenska (Ísland)" },
                new LanguageOption() { Id = 89, FullLanguageCode = "it-CH", ShortLanguageCode = "it", LanguageName = "Italian (Switzerland)", NativeLanguageName = "Italiano (Svizzera)" },
                new LanguageOption() { Id = 90, FullLanguageCode = "it-IT", ShortLanguageCode = "it", LanguageName = "Italian (Italy)", NativeLanguageName = "Italiano (Italia)" },
                new LanguageOption() { Id = 91, FullLanguageCode = "ja-JP", ShortLanguageCode = "ja", LanguageName = "Japanese (Japan)", NativeLanguageName = "日本語 (日本)" },
                new LanguageOption() { Id = 92, FullLanguageCode = "ka-GE", ShortLanguageCode = "ka", LanguageName = "Georgian (Georgia)", NativeLanguageName = "ქართული (საქართველო)" },
                new LanguageOption() { Id = 93, FullLanguageCode = "kk-KZ", ShortLanguageCode = "kk", LanguageName = "Kazakh (Kazakhstan)", NativeLanguageName = "Қазақ (Қазақстан)" },
                new LanguageOption() { Id = 94, FullLanguageCode = "km-KH", ShortLanguageCode = "km", LanguageName = "Khmer (Cambodia)", NativeLanguageName = "ខ្មែរ (កម្ពុជា)" },
                new LanguageOption() { Id = 95, FullLanguageCode = "kn-IN", ShortLanguageCode = "kn", LanguageName = "Kannada (India)", NativeLanguageName = "ಕನ್ನಡ (ಭಾರತ)" },
                new LanguageOption() { Id = 96, FullLanguageCode = "ko-KR", ShortLanguageCode = "ko", LanguageName = "Korean (South Korea)", NativeLanguageName = "한국어 (대한민국)" },
                new LanguageOption() { Id = 97, FullLanguageCode = "ky-KG", ShortLanguageCode = "ky", LanguageName = "Kyrgyz (Kyrgyzstan)", NativeLanguageName = "Кыргыз (Кыргызстан)" },
                new LanguageOption() { Id = 98, FullLanguageCode = "lo-LA", ShortLanguageCode = "lo", LanguageName = "Lao (Laos)", NativeLanguageName = "ລາວ (ລາວ)" },
                new LanguageOption() { Id = 99, FullLanguageCode = "lt-LT", ShortLanguageCode = "lt", LanguageName = "Lithuanian (Lithuania)", NativeLanguageName = "Lietuvių (Lietuva)" },
                new LanguageOption() { Id = 100, FullLanguageCode = "lv-LV", ShortLanguageCode = "lv", LanguageName = "Latvian (Latvia)", NativeLanguageName = "Latviešu (Latvija)" },
                new LanguageOption() { Id = 101, FullLanguageCode = "mk-MK", ShortLanguageCode = "mk", LanguageName = "Macedonian (North Macedonia)", NativeLanguageName = "Македонски (Северна Македонија)" },
                new LanguageOption() { Id = 102, FullLanguageCode = "ml-IN", ShortLanguageCode = "ml", LanguageName = "Malayalam (India)", NativeLanguageName = "മലയാളം (ഇന്ത്യ)" },
                new LanguageOption() { Id = 103, FullLanguageCode = "mn-MN", ShortLanguageCode = "mn", LanguageName = "Mongolian (Mongolia)", NativeLanguageName = "Монгол (Монгол)" },
                new LanguageOption() { Id = 104, FullLanguageCode = "mr-IN", ShortLanguageCode = "mr", LanguageName = "Marathi (India)", NativeLanguageName = "मराठी (भारत)" },
                new LanguageOption() { Id = 105, FullLanguageCode = "ms-MY", ShortLanguageCode = "ms", LanguageName = "Malay (Malaysia)", NativeLanguageName = "Bahasa Melayu (Malaysia)" },
                new LanguageOption() { Id = 106, FullLanguageCode = "my-MM", ShortLanguageCode = "my", LanguageName = "Burmese (Myanmar)", NativeLanguageName = "ဗမာ (မြန်မာ)" },
                new LanguageOption() { Id = 107, FullLanguageCode = "nb-NO", ShortLanguageCode = "nb", LanguageName = "Norwegian Bokmål (Norway)", NativeLanguageName = "Norsk bokmål (Norge)" },
                new LanguageOption() { Id = 108, FullLanguageCode = "ne-NP", ShortLanguageCode = "ne", LanguageName = "Nepali (Nepal)", NativeLanguageName = "नेपाली (नेपाल)" },
                new LanguageOption() { Id = 109, FullLanguageCode = "nl-BE", ShortLanguageCode = "nl", LanguageName = "Dutch (Belgium)", NativeLanguageName = "Nederlands (België)" },
                new LanguageOption() { Id = 110, FullLanguageCode = "nl-NL", ShortLanguageCode = "nl", LanguageName = "Dutch (Netherlands)", NativeLanguageName = "Nederlands (Nederland)" },
                new LanguageOption() { Id = 111, FullLanguageCode = "nn-NO", ShortLanguageCode = "nn", LanguageName = "Norwegian Nynorsk (Norway)", NativeLanguageName = "Norsk nynorsk (Norge)" },
                new LanguageOption() { Id = 112, FullLanguageCode = "or-IN", ShortLanguageCode = "or", LanguageName = "Odia (India)", NativeLanguageName = "ଓଡ଼ିଆ (ଭାରତ)" },
                new LanguageOption() { Id = 113, FullLanguageCode = "pa-IN", ShortLanguageCode = "pa", LanguageName = "Punjabi (India)", NativeLanguageName = "ਪੰਜਾਬੀ (ਭਾਰਤ)" },
                new LanguageOption() { Id = 114, FullLanguageCode = "pl-PL", ShortLanguageCode = "pl", LanguageName = "Polish (Poland)", NativeLanguageName = "Polski (Polska)" },
                new LanguageOption() { Id = 115, FullLanguageCode = "ps-AF", ShortLanguageCode = "ps", LanguageName = "Pashto (Afghanistan)", NativeLanguageName = "پښتو (افغانستان)" },
                new LanguageOption() { Id = 116, FullLanguageCode = "pt-BR", ShortLanguageCode = "pt", LanguageName = "Portuguese (Brazil)", NativeLanguageName = "Português (Brasil)" },
                new LanguageOption() { Id = 117, FullLanguageCode = "pt-PT", ShortLanguageCode = "pt", LanguageName = "Portuguese (Portugal)", NativeLanguageName = "Português (Portugal)" },
                new LanguageOption() { Id = 118, FullLanguageCode = "ro-RO", ShortLanguageCode = "ro", LanguageName = "Romanian (Romania)", NativeLanguageName = "Română (România)" },
                new LanguageOption() { Id = 119, FullLanguageCode = "ru-RU", ShortLanguageCode = "ru", LanguageName = "Russian (Russia)", NativeLanguageName = "Русский (Россия)" },
                new LanguageOption() { Id = 120, FullLanguageCode = "si-LK", ShortLanguageCode = "si", LanguageName = "Sinhala (Sri Lanka)", NativeLanguageName = "සිංහල (ශ්රී ලංකාව)" },
                new LanguageOption() { Id = 121, FullLanguageCode = "sk-SK", ShortLanguageCode = "sk", LanguageName = "Slovak (Slovakia)", NativeLanguageName = "Slovenčina (Slovensko)" },
                new LanguageOption() { Id = 122, FullLanguageCode = "sl-SI", ShortLanguageCode = "sl", LanguageName = "Slovenian (Slovenia)", NativeLanguageName = "Slovenščina (Slovenija)" },
                new LanguageOption() { Id = 123, FullLanguageCode = "sq-AL", ShortLanguageCode = "sq", LanguageName = "Albanian (Albania)", NativeLanguageName = "Shqip (Shqipëria)" },
                new LanguageOption() { Id = 124, FullLanguageCode = "sr-RS", ShortLanguageCode = "sr", LanguageName = "Serbian (Cyrillic, Serbia)", NativeLanguageName = "Српски (Србија)" },
                new LanguageOption() { Id = 125, FullLanguageCode = "su-ID", ShortLanguageCode = "su", LanguageName = "Sundanese (Indonesia)", NativeLanguageName = "Basa Sunda (Indonesia)" },
                new LanguageOption() { Id = 126, FullLanguageCode = "sv-SE", ShortLanguageCode = "sv", LanguageName = "Swedish (Sweden)", NativeLanguageName = "Svenska (Sverige)" },
                new LanguageOption() { Id = 127, FullLanguageCode = "sw-KE", ShortLanguageCode = "sw", LanguageName = "Swahili (Kenya)", NativeLanguageName = "Kiswahili (Kenya)" },
                new LanguageOption() { Id = 128, FullLanguageCode = "ta-IN", ShortLanguageCode = "ta", LanguageName = "Tamil (India)", NativeLanguageName = "தமிழ் (இந்தியா)" },
                new LanguageOption() { Id = 129, FullLanguageCode = "te-IN", ShortLanguageCode = "te", LanguageName = "Telugu (India)", NativeLanguageName = "తెలుగు (భారతదేశం)" },
                new LanguageOption() { Id = 130, FullLanguageCode = "tg-Cyrl-TJ", ShortLanguageCode = "tg", LanguageName = "Tajik (Cyrillic, Tajikistan)", NativeLanguageName = "Тоҷикӣ (Тоҷикистон)" },
                new LanguageOption() { Id = 131, FullLanguageCode = "th-TH", ShortLanguageCode = "th", LanguageName = "Thai (Thailand)", NativeLanguageName = "ไทย (ประเทศไทย)" },
                new LanguageOption() { Id = 132, FullLanguageCode = "tk-TM", ShortLanguageCode = "tk", LanguageName = "Turkmen (Turkmenistan)", NativeLanguageName = "Türkmen (Türkmenistan)" },
                new LanguageOption() { Id = 133, FullLanguageCode = "tr-TR", ShortLanguageCode = "tr", LanguageName = "Turkish (Turkey)", NativeLanguageName = "Türkçe (Türkiye)" },
                new LanguageOption() { Id = 134, FullLanguageCode = "tt-RU", ShortLanguageCode = "tt", LanguageName = "Tatar (Russia)", NativeLanguageName = "Татар (Россия)" },
                new LanguageOption() { Id = 135, FullLanguageCode = "ug-CN", ShortLanguageCode = "ug", LanguageName = "Uighur (China)", NativeLanguageName = "ئۇيغۇرچە (جۇڭگو)" },
                new LanguageOption() { Id = 136, FullLanguageCode = "uk-UA", ShortLanguageCode = "uk", LanguageName = "Ukrainian (Ukraine)", NativeLanguageName = "Українська (Україна)" },
                new LanguageOption() { Id = 137, FullLanguageCode = "ur-PK", ShortLanguageCode = "ur", LanguageName = "Urdu (Pakistan)", NativeLanguageName = "اردو (پاکستان)" },
                new LanguageOption() { Id = 138, FullLanguageCode = "uz-Cyrl-UZ", ShortLanguageCode = "uz", LanguageName = "Uzbek (Cyrillic, Uzbekistan)", NativeLanguageName = "Ўзбек (Ўзбекистон)" },
                new LanguageOption() { Id = 139, FullLanguageCode = "vi-VN", ShortLanguageCode = "vi", LanguageName = "Vietnamese (Vietnam)", NativeLanguageName = "Tiếng Việt (Việt Nam)" },
                new LanguageOption() { Id = 140, FullLanguageCode = "zh-CN", ShortLanguageCode = "zh", LanguageName = "Chinese (Simplified, China)", NativeLanguageName = "中文(简体，中国)" },
                new LanguageOption() { Id = 141, FullLanguageCode = "zh-HK", ShortLanguageCode = "zh", LanguageName = "Chinese (Traditional, Hong Kong SAR)", NativeLanguageName = "中文(繁體，香港特別行政區)" },
                new LanguageOption() { Id = 142, FullLanguageCode = "zh-TW", ShortLanguageCode = "zh", LanguageName = "Chinese (Traditional, Taiwan)", NativeLanguageName = "中文(繁體，台灣)" }

            }.OrderBy(x => x.LanguageName).ToList();
        }
    }
}
