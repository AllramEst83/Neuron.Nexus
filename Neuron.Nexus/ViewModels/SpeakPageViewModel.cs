using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Neuron.Nexus.Models;
using System.Collections.ObjectModel;
using System.Threading;

namespace Neuron.Nexus.ViewModels;
[QueryProperty(nameof(LanguageOneToBeSent), "languageOneToBeSent")]
[QueryProperty(nameof(LanguageTwoToBeSent), "languageTwoToBeSent")]
public partial class SpeakPageViewModel : BaseViewModel
{
    public string LanguageOneToBeSent
    {
        set
        {
            LanguageOne = Newtonsoft.Json.JsonConvert.DeserializeObject<LanguageOption>(Uri.UnescapeDataString(value));
            // Now you can use yourObject
        }
    }
    public string LanguageTwoToBeSent
    {
        set
        {
            LanguageTwo = Newtonsoft.Json.JsonConvert.DeserializeObject<LanguageOption>(Uri.UnescapeDataString(value));
            // Now you can use yourObject
        }
    }

    [ObservableProperty]
    private LanguageOption languageOne = null;
    [ObservableProperty]
    private LanguageOption languageTwo = null;
    [ObservableProperty]
    private bool isSpeakButtonsEnabled = false;
    [ObservableProperty]
    string recognitionTextOne = "";
    [ObservableProperty]
    string recognitionTextTwo = "";

    private ObservableCollection<UserMessage> _userMessages;
    public ObservableCollection<UserMessage> UserMessages
    {
        get => _userMessages;
        set => SetProperty(ref _userMessages, value);
    }

    private readonly ISpeechToText _speechToText;

    public SpeakPageViewModel(ISpeechToText speechToText)
    {
        _speechToText = speechToText;
        UserMessages = new ObservableCollection<UserMessage>();

        IsSpeakButtonsEnabled = !IsSpeakButtonsEnabled;

    }

    [RelayCommand]
    static void Stop()
    {
        WeakReferenceMessenger.Default.Send(new AnimateButtonMessage(AnimationButtonsEnum.StopBtn));

    }

    [RelayCommand(IncludeCancelCommand = true)]
    async Task SpeakLanguageTwo(CancellationToken cancellationToken)
    {
        WeakReferenceMessenger.Default.Send(new AnimateButtonMessage(AnimationButtonsEnum.LanguageTwoBtn));

        System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo(LanguageTwo.FullLanguageCode.ToLower() ?? "en-US");
        var progressSettings = new Progress<string>();

        var recognitionResult = await _speechToText.ListenAsync(culture, progressSettings, cancellationToken);

        if (recognitionResult.IsSuccessful)
        {
            RecognitionTextOne = recognitionResult.Text;

            UserMessages.Add(new UserMessage()
            {
                User = 2,
                ChatMessage = RecognitionTextOne
            });
        }
        else
        {
            await Toast.Make(recognitionResult.Exception?.Message ?? "Unable to recognize speech", ToastDuration.Long).Show(CancellationToken.None);
        }
    }

    [RelayCommand(IncludeCancelCommand = true)]
    async Task SpeakLanguageOne(CancellationToken cancellationToken)
    {
        WeakReferenceMessenger.Default.Send(new AnimateButtonMessage(AnimationButtonsEnum.LanguageOneBtn));

        System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo(LanguageOne.FullLanguageCode.ToLower() ?? "en-US");
        var progressSettings = new Progress<string>();

        var recognitionResult = await _speechToText.ListenAsync(culture, progressSettings, cancellationToken);

        if (recognitionResult.IsSuccessful)
        {
            RecognitionTextOne = recognitionResult.Text;

            UserMessages.Add(new UserMessage()
            {
                User = 1,
                ChatMessage = RecognitionTextOne
            });

            System.Diagnostics.Debug.WriteLine("Sending NewMessageAdded message");
            WeakReferenceMessenger.Default.Send("NewMessageAdded");
        }
        else
        {
            await Toast.Make(recognitionResult.Exception?.Message ?? "Unable to recognize speech", ToastDuration.Long).Show(CancellationToken.None);
        }
    }

    public async Task Initialize()
    {
        //...
    }

}


