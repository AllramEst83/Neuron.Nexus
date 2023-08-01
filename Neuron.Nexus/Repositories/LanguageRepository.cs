using Neuron.Nexus.Models;

namespace Neuron.Nexus.Repositories
{
    public interface ILanguageRepository
    {
        List<LanguageOption> GetLanguagesOrderByLanguageName();
    }
    public class LanguageRepository : ILanguageRepository
    {
        public LanguageRepository()
        {
        }

        /// <summary>
        /// Compatible langugages
        /// https://learn.microsoft.com/en-us/azure/ai-services/speech-service/language-support?tabs=stt
        /// </summary>
        /// <returns></returns>
        public List<LanguageOption> GetLanguagesOrderByLanguageName()
        {
            //Svenksa
            //Norska
            //Danska
            //Finska
            //English

            //Franska
            //Tyska
            //Spanska
            //Italienska

            //Arabic(Syria)
            //Arabic(Iraq)
            //Arabic(Iran,Persiska)
            //Turkiska

            //Kurdiska
            //Ukrainska

            //somaliska
            //Amharic(Ethiopia)

            //Polska
            //Rysska

            //Romani
            //Bosniska
            //Kroatiska
            //Serbiska
            return new List<LanguageOption>
            {
                new LanguageOption() {Id = 1,FullLanguageCode = "sv-SE", ShortLanguageCode = "sv" ,LanguageName = "Swedish", NativeLanguageName = "Svenska" },
                new LanguageOption() {Id = 13,FullLanguageCode = "nb-NO", ShortLanguageCode = "nb", LanguageName = "Norwegian Bokmål ", NativeLanguageName = "Norsk"},
                new LanguageOption() {Id = 13,FullLanguageCode = "da-DK", ShortLanguageCode = "da", LanguageName = "Danish ", NativeLanguageName = "Dansk"},
                new LanguageOption() {Id = 10,FullLanguageCode = "fi-FI", ShortLanguageCode = "fi", LanguageName = "Finnish", NativeLanguageName = "Suomi"},
                new LanguageOption() {Id = 2,FullLanguageCode = "en-US", ShortLanguageCode = "en", LanguageName = "English", NativeLanguageName = "English"},
                
                new LanguageOption() {Id = 5,FullLanguageCode = "fr-FR", ShortLanguageCode = "fr", LanguageName = "French ", NativeLanguageName = "Français"},
                new LanguageOption() {Id = 7,FullLanguageCode = "de-DE", ShortLanguageCode = "de", LanguageName = "German ", NativeLanguageName = "Deutsch"},
                new LanguageOption() {Id = 4,FullLanguageCode = "es-ES", ShortLanguageCode = "es", LanguageName = "Spanish ", NativeLanguageName = "Español "},
                new LanguageOption() {Id = 12,FullLanguageCode = "it-IT", ShortLanguageCode = "it", LanguageName = "Italian", NativeLanguageName = "Italia" },
                
                new LanguageOption() {Id = 11,FullLanguageCode = "ar-IQ", ShortLanguageCode = "ar", LanguageName = "Arabic (Iraq)", NativeLanguageName = "al-ʿIrāq"},
                new LanguageOption() {Id = 11,FullLanguageCode = "ar-SY", ShortLanguageCode = "ar", LanguageName = "Arabic (Syria)", NativeLanguageName = "Syria"},
                new LanguageOption() {Id = 11,FullLanguageCode = "fa-IR", ShortLanguageCode = "fa", LanguageName = "Persian (Iran)", NativeLanguageName = "Iran"},
                new LanguageOption() {Id = 3,FullLanguageCode = "tr-TR", ShortLanguageCode = "tr", LanguageName = "Turkish ", NativeLanguageName = "Türkçesi "},

                new LanguageOption() {Id = 9,FullLanguageCode = "uk-UA", ShortLanguageCode = "uk", LanguageName = "Ukrainian", NativeLanguageName = "Ukrajina"},
                new LanguageOption() {Id = 6,FullLanguageCode = "pl-PL", ShortLanguageCode = "pl", LanguageName = "Polish (Poland)", NativeLanguageName = "Polish"},
                new LanguageOption() {Id = 8,FullLanguageCode = "ru-RU", ShortLanguageCode = "ru", LanguageName = "Russian", NativeLanguageName = "Russkie "},

                new LanguageOption() {Id = 6,FullLanguageCode = "so-SO", ShortLanguageCode = "so", LanguageName = "Somali (Somalia)", NativeLanguageName = "Somali"},
                new LanguageOption() {Id = 13,FullLanguageCode = "am-ET", ShortLanguageCode = "am", LanguageName = "Amharic (Ethiopia)", NativeLanguageName = "Amharic"},

                new LanguageOption() {Id = 6,FullLanguageCode = "ro-RO", ShortLanguageCode = "ro", LanguageName = "Romanian (Romania)", NativeLanguageName = "Romanian"},
                new LanguageOption() {Id = 6,FullLanguageCode = "bs-BA", ShortLanguageCode = "bs", LanguageName = "Bosnian", NativeLanguageName = "Bosnia"},                
                new LanguageOption() {Id = 13,FullLanguageCode = "hr-HR", ShortLanguageCode = "hr", LanguageName = "Croatian", NativeLanguageName = "Hrvatska"},
                new LanguageOption() {Id = 13,FullLanguageCode = "sr-RS", ShortLanguageCode = "hsr", LanguageName = "Serbian (Cyrillic, Serbia)", NativeLanguageName = "Serbian"}

            }.OrderBy(x => x.LanguageName).ToList();
        }
    }
}
