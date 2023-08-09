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
                new LanguageOption() { Id = 1, FullLanguageCode = "fa-IR", ShortLanguageCode = "fa", LanguageName = "Persian (Iran)", NativeLanguageName = "Dari" },
                new LanguageOption() { Id = 2, FullLanguageCode = "ar-IQ", ShortLanguageCode = "ar", LanguageName = "Arabic (Iraq)", NativeLanguageName = "Iraq" },
                new LanguageOption() { Id = 3, FullLanguageCode = "ar-SY", ShortLanguageCode = "ar", LanguageName = "Arabic (Syria)", NativeLanguageName = "Syria" },
                new LanguageOption() { Id = 4, FullLanguageCode = "tr-TR", ShortLanguageCode = "tr", LanguageName = "Turkish (Turkey)", NativeLanguageName = "Türkiye" },
                new LanguageOption() { Id = 5, FullLanguageCode = "uk-UA", ShortLanguageCode = "uk", LanguageName = "Ukrainian (Ukraine)", NativeLanguageName = "Україна" },
                new LanguageOption() { Id = 6, FullLanguageCode = "lt-LT", ShortLanguageCode = "lt", LanguageName = "Lithuanian (Lithuania)", NativeLanguageName = "Lietuva" },
                new LanguageOption() { Id = 7, FullLanguageCode = "lv-LV", ShortLanguageCode = "lv", LanguageName = "Latvian (Latvia)", NativeLanguageName = "Latvija" },
                new LanguageOption() { Id = 8, FullLanguageCode = "th-TH", ShortLanguageCode = "th", LanguageName = "Thai (Thailand)", NativeLanguageName = "ไทย" },
                new LanguageOption() { Id = 9, FullLanguageCode = "ru-RU", ShortLanguageCode = "ru", LanguageName = "Russian (Russia)", NativeLanguageName = "Россия" },
                new LanguageOption() { Id = 10, FullLanguageCode = "pl-PL", ShortLanguageCode = "pl", LanguageName = "Polish (Poland)", NativeLanguageName = "Polska" },
                new LanguageOption() { Id = 11, FullLanguageCode = "so-SO", ShortLanguageCode = "so", LanguageName = "Somali (Somalia)", NativeLanguageName = "Soomaaliya" },
                new LanguageOption() {Id = 12,FullLanguageCode = "sv-SE", ShortLanguageCode = "sv" ,LanguageName = "Sweden (Swedish)", NativeLanguageName = "Svenska" },
                new LanguageOption() {Id = 13,FullLanguageCode = "en-US", ShortLanguageCode = "en", LanguageName = "USA (English)", NativeLanguageName = "English"}

            }.OrderBy(x => x.LanguageName).ToList();
        }
    }
}
