using CommunityToolkit.Mvvm.ComponentModel;

namespace Neuron.Nexus.Models
{
    public partial class UserMessage : ObservableObject
    {
        [ObservableProperty]
        public string chatMessage;
        public int User { get; set; }
        public string TranslatedLanguage { get; set; }
        [ObservableProperty]
        public string spokenText;
        public string SpokenLanguage { get; set; }
    }
}
