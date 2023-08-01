using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Neuron.Nexus.Models;
using Neuron.Nexus.Pages;
using Neuron.Nexus.Services;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

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
        private readonly IUserPersmissionsService userPersmissionsService;

        public SelectLanguagePageViewModel(ISpeechToText speecheTotext, ILanguageService languageService, IUserPersmissionsService userPersmissionsService)
        {
            _speechToText = speecheTotext;
            _languageService = languageService;
            this.userPersmissionsService = userPersmissionsService;
        }

        [RelayCommand(IncludeCancelCommand = true)]
        public async Task Start(CancellationToken cancellationToken)
        {
            if (await userPersmissionsService.GetPermissionsFromUser(cancellationToken))
            {
                string languageOneToBeSent = JsonConvert.SerializeObject(SelectedLanguageOne);
                string languageTwoToBeSent = JsonConvert.SerializeObject(SelectedLanguageTwo);
                    
                await Shell.Current.GoToAsync($"{nameof(SpeakPage)}?languageOneToBeSent={Uri.EscapeDataString(languageOneToBeSent)}&languageTwoToBeSent={Uri.EscapeDataString(languageTwoToBeSent)}");
            }
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
            Languages = new ObservableCollection<LanguageOption>(_languageService.GetLanguages());
            if (IsStartButtonEnabled)
            {
                IsStartButtonEnabled = !IsStartButtonEnabled;
            }
        }
    }
}
