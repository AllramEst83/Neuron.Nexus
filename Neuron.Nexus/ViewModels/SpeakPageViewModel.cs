using CommunityToolkit.Mvvm.Messaging;
using Neuron.Nexus.Models;
using Neuron.Nexus.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Neuron.Nexus.ViewModels;
public class SpeakPageViewModel : BaseViewModel
{
    private string _languageOne;
    private string _languageTwo;
    private bool _isStartButtonEnabled = false;
    private bool _isSpeakButtonsEnabled = false;
    private bool _isLanguageSelectionVisible = true;

    public ICommand SpeakLanguageOneCommand { get; private set; }
    public ICommand SpeakLanguageTwoCommand { get; private set; }
    public ICommand StopCommand { get; private set; }
    public ICommand StartCommand { get; private set; }
    public ICommand SelectedIndexChangedCommandOne { get; private set; }
    public ICommand SelectedIndexChangedCommandTwo { get; private set; }

    public ObservableCollection<string> Languages { get; set; }

    public bool IsStartButtonEnabled
    {
        get { return _isStartButtonEnabled; }
        set
        {
            _isStartButtonEnabled = value;
            OnPropertyChanged(nameof(IsStartButtonEnabled));
        }
    }

    public bool IsSpeakButtonsEnabled
    {
        get { return _isSpeakButtonsEnabled; }
        set
        {
            _isSpeakButtonsEnabled = value;
            OnPropertyChanged(nameof(IsSpeakButtonsEnabled));
        }
    }

    private readonly ILanguageService _languageService;

    public bool IsLanguageSelectionVisible
    {
        get { return _isLanguageSelectionVisible; }
        set
        {
            _isLanguageSelectionVisible = value;
            OnPropertyChanged(nameof(IsLanguageSelectionVisible));
        }
    }

    public SpeakPageViewModel(ILanguageService languageService)
    {
        _languageService = languageService;

        SpeakLanguageOneCommand = new Command(() =>
                    WeakReferenceMessenger.Default.Send(new AnimateButtonMessage(AnimationButtonsEnum.LanguageOneBtn)));

        SpeakLanguageTwoCommand = new Command(() =>
            WeakReferenceMessenger.Default.Send(new AnimateButtonMessage(AnimationButtonsEnum.LanguageTwoBtn)));

        StopCommand = new Command(() =>
            WeakReferenceMessenger.Default.Send(new AnimateButtonMessage(AnimationButtonsEnum.StopBtn)));

        StartCommand = new Command(ToggleLanguageSelectionVisibility);

        SelectedIndexChangedCommandOne = new Command<Picker>(HandlePickerSelectionChangedOne);
        SelectedIndexChangedCommandTwo = new Command<Picker>(HandlePickerSelectionChangedTwo);

        Languages = new ObservableCollection<string>(_languageService.GetLanguages());
    }

    private void HandlePickerSelectionChangedOne(Picker picker)
    {
        SetLanguageOne(picker.SelectedItem as string);
        UpdateStartButtonState();
    }

    private void HandlePickerSelectionChangedTwo(Picker picker)
    {
        SetLanguageTwo(picker.SelectedItem as string);
        UpdateStartButtonState();
    }

    private void ToggleLanguageSelectionVisibility()
    {
        IsLanguageSelectionVisible = !IsLanguageSelectionVisible;
        IsSpeakButtonsEnabled = !IsSpeakButtonsEnabled;
    }

    private void SetLanguageOne(string language)
    {
        _languageOne = language;
    }

    private void SetLanguageTwo(string language)
    {
        _languageTwo = language;
    }

    private void UpdateStartButtonState()
    {
        IsStartButtonEnabled = !string.IsNullOrEmpty(_languageOne) && !string.IsNullOrEmpty(_languageTwo);
    }
}

