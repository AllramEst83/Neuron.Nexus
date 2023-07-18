using Neuron.Nexus.Models;
using Neuron.Nexus.Repositories;
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
        List<LanguageOption> GetLanguages();
    }
    public class LanguageService : ILanguageService
    {
        public LanguageService(ILanguageRepository languageRepository)
        {
           _languageRepository = languageRepository;
        }

        public ILanguageRepository _languageRepository { get; }

        public List<LanguageOption> GetLanguages()
        {
            return _languageRepository.GetLanguages();
        }
    }
}
