using Neuron.Nexus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Neuron.Nexus.Repositories
{
    public interface ILanguageRepository
    {
        List<LanguageOption> GetLanguages();
    }
    public class LanguageRepository : ILanguageRepository
    {
        public LanguageRepository()
        {
        }
        public List<LanguageOption> GetLanguages()
        {
            return new List<LanguageOption>
            {
                new LanguageOption() {Id = 1, FullLanguageCode = "sv-SE", LanguageName = "Swedish" },
                new LanguageOption() {Id = 2,FullLanguageCode = "en-US", LanguageName = "English"}
            };
        }
    }
}
