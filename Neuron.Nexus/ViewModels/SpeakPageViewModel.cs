using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Neuron.Nexus.Models;
using System.Collections.ObjectModel;

namespace Neuron.Nexus.ViewModels;
[QueryProperty(nameof(LanguageOneToBeSent), "languageOneToBeSent")]
[QueryProperty(nameof(LanguageTwoToBeSent), "languageTwoToBeSent")]
public partial class SpeakPageViewModel : BaseViewModel
{
    public string LanguageOneToBeSent
    {
        set
        {
            LanguageOne= Newtonsoft.Json.JsonConvert.DeserializeObject<Language>(Uri.UnescapeDataString(value));
            // Now you can use yourObject
        }
    }
    public string LanguageTwoToBeSent
    {
        set
        {
            LanguageTwo = Newtonsoft.Json.JsonConvert.DeserializeObject<Language>(Uri.UnescapeDataString(value));
            // Now you can use yourObject
        }
    }

    [ObservableProperty]
    private Language languageOne = null;
    [ObservableProperty]
    private Language languageTwo = null;
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
       

       
        UserMessages = new ObservableCollection<UserMessage>
        {
            new UserMessage { User = 1, ChatMessage = "Hello" },
            new UserMessage { User = 2, ChatMessage = "Hello back" },
             new UserMessage { User = 1, ChatMessage = "Hello" },
              new UserMessage { User = 1, ChatMessage = "Hello" },
               new UserMessage { User = 2, ChatMessage = "Hello" },
                new UserMessage { User = 1, ChatMessage = "Hello" },
                 new UserMessage { User = 1, ChatMessage = "Hello" },

                  new UserMessage { User = 1, ChatMessage = "Hello" },
                  new UserMessage { User = 1, ChatMessage = "Hello" },
                 new UserMessage { User = 1, ChatMessage = "Hello" },

                 new UserMessage { User = 1, ChatMessage = "Hello" },
                 new UserMessage { User = 2, ChatMessage = "Hello" },
                 new UserMessage { User = 2, ChatMessage = "Hello" },
                   new UserMessage { User = 1, ChatMessage = "Hello" },
            new UserMessage { User = 2, ChatMessage = "Hello back" },
             new UserMessage { User = 1, ChatMessage = "Hello" },
              new UserMessage { User = 1, ChatMessage = "Hello" },
               new UserMessage { User = 2, ChatMessage = "Hello" },
                new UserMessage { User = 1, ChatMessage = "Hello" },
                 new UserMessage { User = 1, ChatMessage = "Hello" },

                  new UserMessage { User = 1, ChatMessage = "Hello" },
                  new UserMessage { User = 1, ChatMessage = "Hello" },
                 new UserMessage { User = 1, ChatMessage = "Hello" },

                 new UserMessage { User = 1, ChatMessage = "Hello" },
                 new UserMessage { User = 2, ChatMessage = "Hello" },
                 new UserMessage { User = 2, ChatMessage = "Hello" }
        };

        IsSpeakButtonsEnabled = !IsSpeakButtonsEnabled;

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

    [RelayCommand(IncludeCancelCommand = true)]
    async Task SpeakLanguageOne(CancellationToken cancellationToken)
    {
        WeakReferenceMessenger.Default.Send(new AnimateButtonMessage(AnimationButtonsEnum.LanguageOneBtn));

        System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo(LanguageOne.FullLanguageCode.ToLower() ?? "en-US");
        var progressSettings = new Progress<string>(partialText =>
        {
            RecognitionTextOne += partialText;
        });

        var recognitionResult = await _speechToText.ListenAsync(culture, progressSettings, cancellationToken);

        if (recognitionResult.IsSuccessful)
        {
            RecognitionTextOne = recognitionResult.Text;
        }
        else
        {
            await Toast.Make(recognitionResult.Exception?.Message ?? "Unable to recognize speech", ToastDuration.Long).Show(CancellationToken.None);
        }
    }
}


