using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Neuron.Nexus.Models;
using Neuron.Nexus.Pages;
using Neuron.Nexus.Services;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Neuron.Nexus.ViewModels
{
    public partial class SelectLanguagePageViewModel : BaseViewModel
    {
        [ObservableProperty]
        private Language selectedLanguageOne = null;
        [ObservableProperty]
        private Language selectedLanguageTwo = null;
        [ObservableProperty]
        private bool isStartButtonEnabled = false;
        public ObservableCollection<Language> Languages { get; set; }
        private readonly ISpeechToText _speechToText;
        private readonly ILanguageService _languageService;

        public ICommand NavigateToSpeakCommand { get; private set; }

        public SelectLanguagePageViewModel(ISpeechToText speecheTotext, ILanguageService languageService)
        {
            _speechToText = speecheTotext;
            _languageService = languageService;

            Languages = new ObservableCollection<Language>(_languageService.GetLanguages());

            NavigateToSpeakCommand = new Command<CancellationToken>(async (ct) => await NavigateToSpeakAsync(ct));
        }

        [RelayCommand(IncludeCancelCommand = true)]
        public async Task Start(CancellationToken cancellationToken)
        {
            var isGranted = await _speechToText.RequestPermissions(cancellationToken);
            if (!isGranted)
            {
                await Toast.Make("Permission not granted").Show(CancellationToken.None);
                return;
            }

            NavigateToSpeakCommand.Execute(cancellationToken);
        }

        [RelayCommand]
        private void HandlePickerSelectionChanged(Picker picker)
        {
            if (SelectedLanguageOne is not null && SelectedLanguageTwo is not null)
            {
                IsStartButtonEnabled = !IsStartButtonEnabled;
            }
        }

        private async Task NavigateToSpeakAsync(CancellationToken cancellationToken)
        {
            string languageOneToBeSent = JsonConvert.SerializeObject(SelectedLanguageOne);
            string languageTwoToBeSent = JsonConvert.SerializeObject(SelectedLanguageOne);

            await Shell.Current.GoToAsync($"//{nameof(SpeakPage)}?languageOneToBeSent={Uri.EscapeDataString(languageOneToBeSent)}&languageTwoToBeSent={Uri.EscapeDataString(languageTwoToBeSent)}");
        }
    }
}
