using Neuron.Nexus.Models;
using Neuron.Nexus.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuron.Nexus.Services
{
    public interface ILanguageService
    {
        List<Language> GetLanguages();
    }
    public class LanguageService : ILanguageService
    {
        public List<Language> GetLanguages()
        {
            return new List<Language>()
            {
                new Language() { Id = 1 , LanguageName = "Swedish" , FullLanguageCode = "sv-SE"},
                new Language() { Id = 2 , LanguageName = "English" , FullLanguageCode = "en-US"}
            };
        }
    }
}
