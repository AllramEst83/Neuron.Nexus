using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Media;
using Neuron.Nexus.Models;
using Neuron.Nexus.Pages;
using Neuron.Nexus.Services;
using Newtonsoft.Json;
using Sentry;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Neuron.Nexus.ViewModels
{
    public partial class SelectLanguagePageViewModel : BaseViewModel
    {
        [ObservableProperty]
        private LanguageOption selectedLanguageOne = null;
        [ObservableProperty]
        private LanguageOption selectedLanguageTwo = null;
        [ObservableProperty]
        private bool isStartButtonEnabled = false;
        private ObservableCollection<LanguageOption> _languages;
        public ObservableCollection<LanguageOption> Languages
        {
            get => _languages;
            set => SetProperty(ref _languages, value);
        }

        private readonly ISpeechToText _speechToText;
        private readonly ILanguageService _languageService;


        public SelectLanguagePageViewModel(ISpeechToText speecheTotext, ILanguageService languageService)
        {
            _speechToText = speecheTotext;
            _languageService = languageService;
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

                string languageOneToBeSent = JsonConvert.SerializeObject(SelectedLanguageOne);
                string languageTwoToBeSent = JsonConvert.SerializeObject(SelectedLanguageTwo);

                await Shell.Current.GoToAsync($"{nameof(SpeakPage)}?languageOneToBeSent={Uri.EscapeDataString(languageOneToBeSent)}&languageTwoToBeSent={Uri.EscapeDataString(languageTwoToBeSent)}");

        }

        [RelayCommand]
        void HandlePickerSelectionChangedOne(Picker picker)
        {
            if (SelectedLanguageOne is not null && SelectedLanguageTwo is not null)
            {
                if (!IsStartButtonEnabled)
                {
                    IsStartButtonEnabled = !IsStartButtonEnabled;
                }
            }
        }

        [RelayCommand]
        void HandlePickerSelectionChangedTwo(Picker picker)
        {
            if (SelectedLanguageOne is not null && SelectedLanguageTwo is not null)
            {
                if (!IsStartButtonEnabled)
                {
                    IsStartButtonEnabled = !IsStartButtonEnabled;
                }
            }
        }

        public void Initialize()
        {
            Languages = new ObservableCollection<LanguageOption>(_languageService.GetLanguages().OrderBy(x => x.LanguageName));
            if (IsStartButtonEnabled)
            {
                IsStartButtonEnabled = !IsStartButtonEnabled;
            }            
        }
    }
}
