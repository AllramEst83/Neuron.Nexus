namespace Neuron.Nexus.Models
{
    public class UserMessage
    {
        public string ChatMessage { get; set; }
        public int User { get; set; }
        public string TranslatedLanguage { get; set; }
        public string SpokenText { get; set; }
        public string SpokenLanguage { get; set; }
    }
}
