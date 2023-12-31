﻿using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Neuron.Nexus.Models;
using Neuron.Nexus.Pages;
using Neuron.Nexus.Resources.Languages;
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
        [ObservableProperty]
        private bool showTutorial = true;
        private ObservableCollection<LanguageOption> _languages;
        public ObservableCollection<LanguageOption> Languages
        {
            get => _languages;
            set => SetProperty(ref _languages, value);
        }
        private bool hasBeenInitialized = false;

        private readonly ISpeechToText _speechToText;
        private readonly ILanguageService _languageService;
        private readonly IUserPersmissionsService userPersmissionsService;

        public SelectLanguagePageViewModel(ISpeechToText speecheTotext, ILanguageService languageService, IUserPersmissionsService userPersmissionsService)
        {
            _speechToText = speecheTotext;
            _languageService = languageService;
            this.userPersmissionsService = userPersmissionsService;

            var userhasSeenTutorial = Preferences.Get("hasSeenTutorial", false);
            if (userhasSeenTutorial)
            {
                ShowTutorial = !ShowTutorial;
            }
        }

        [RelayCommand(IncludeCancelCommand = true)]
        public async Task Start(CancellationToken cancellationToken)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Toast.Make(AppResources.NoInternetConnection, CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
                return;
            }

            if (await userPersmissionsService.GetPermissionsFromUser(cancellationToken))
            {
                string languageOneToBeSent = JsonConvert.SerializeObject(SelectedLanguageOne);
                string languageTwoToBeSent = JsonConvert.SerializeObject(SelectedLanguageTwo);

                await Shell.Current.GoToAsync($"{nameof(SpeakPage)}?languageOneToBeSent={Uri.EscapeDataString(languageOneToBeSent)}&languageTwoToBeSent={Uri.EscapeDataString(languageTwoToBeSent)}");
            }
            else
            {
                await Toast.Make("The app needs microphone permission to work properly.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
            }
        }

        [RelayCommand]
        void HandlePickerSelectionChangedOne(Picker picker)
        {
            if (SelectedLanguageOne is not null && SelectedLanguageTwo is not null)
            {
                if (!IsStartButtonEnabled)
                {
                    IsStartButtonEnabled = true;
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
                    IsStartButtonEnabled = true;
                }
            }
        }

        [RelayCommand]
        async Task GoToTutorialPage()
        {
            Preferences.Set("hasSeenTutorial", true);
            ShowTutorial = !ShowTutorial;

            await Shell.Current.GoToAsync($"//{nameof(TutorialPage)}");
        }
        public void Initialize()
        {
            Languages = new ObservableCollection<LanguageOption>(_languageService.GetLanguages());
            if (IsStartButtonEnabled)
            {
                IsStartButtonEnabled = !IsStartButtonEnabled;
            }
        }

        public void SubscribeToEvents()
        {
            if (!hasBeenInitialized)
            {
                WeakReferenceMessenger.Default.Register<DisposeStartMessage>(this, (r, m) =>
                {
                    UnSubscribeToEvents();
                    hasBeenInitialized = false;
                });

                WeakReferenceMessenger.Default.Register<InitializeStartMessage>(this, (r, m) =>
                {
                    if (!hasBeenInitialized)
                    {
                        Initialize();
                        hasBeenInitialized = !hasBeenInitialized;
                    }
                });
            }
        }

        private void UnSubscribeToEvents()
        {
            WeakReferenceMessenger.Default.Unregister<InitializeStartMessage>(this);
            WeakReferenceMessenger.Default.Unregister<DisposeStartMessage>(this);
        }
    }
}
