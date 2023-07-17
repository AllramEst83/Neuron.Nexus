using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Neuron.Nexus.Models;
using Neuron.Nexus.Services;
using System.Collections.ObjectModel;

namespace Neuron.Nexus.ViewModels;
public partial class SpeakPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private Language languageOne = null;
    [ObservableProperty]
    private Language languageTwo = null;
    [ObservableProperty]
    private bool isStartButtonEnabled = false;
    [ObservableProperty]
    private bool isSpeakButtonsEnabled = false;
    [ObservableProperty]
    private bool isLanguageSelectionVisible = true;

    public ObservableCollection<Language> Languages { get; set; }

    private readonly ILanguageService _languageService;

    public SpeakPageViewModel(ILanguageService languageService)
    {
        _languageService = languageService;
        Languages = new ObservableCollection<Language>(_languageService.GetLanguages());
    }

    [RelayCommand]
   public async Task Start()
    {
        ToggleLanguageSelectionVisibility();
    }

    [RelayCommand]
    static void Stop()
    {
        WeakReferenceMessenger.Default.Send(new AnimateButtonMessage(AnimationButtonsEnum.StopBtn));
    }

    [RelayCommand]
    static void SpeakLanguageTwo()
    {
        WeakReferenceMessenger.Default.Send(new AnimateButtonMessage(AnimationButtonsEnum.LanguageTwoBtn));
    }

    [RelayCommand]
    static void SpeakLanguageOne()
    {
        WeakReferenceMessenger.Default.Send(new AnimateButtonMessage(AnimationButtonsEnum.LanguageOneBtn));
    }

    [RelayCommand]
    private void HandlePickerSelectionChanged(Picker picker)
    {
        if (LanguageOne is not null && LanguageTwo is not null)
        {
            IsStartButtonEnabled = true;
        }
    }

    private void ToggleLanguageSelectionVisibility()
    {
        IsLanguageSelectionVisible = !IsLanguageSelectionVisible;
        IsSpeakButtonsEnabled = !IsSpeakButtonsEnabled;
    } 
}


