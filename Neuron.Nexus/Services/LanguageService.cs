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
        List<string> GetLanguages();
    }
    public class LanguageService: ILanguageService
{

        public List<string> GetLanguages()
        {
            return new List<string>()        {
                "English",
                "Spanish",
                "French"
            };
        }
    } 
}
