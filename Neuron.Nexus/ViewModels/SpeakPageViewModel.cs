using Android.Media;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Translation;
using Neuron.Nexus.Models;
using System.Collections.ObjectModel;
using System.Threading;
using OutputFormat = Microsoft.CognitiveServices.Speech.OutputFormat;

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

            WeakReferenceMessenger.Default.Send("NewMessageAdded");
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

            WeakReferenceMessenger.Default.Send("NewMessageAdded");
        }
        else
        {
            await Toast.Make(recognitionResult.Exception?.Message ?? "Unable to recognize speech", ToastDuration.Long).Show(CancellationToken.None);
        }
    }

    public async Task Initialize()
    {
        await CheckForMicPermission();
        await SetupRecognizerOne();
    }

    private async Task CheckForMicPermission()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.Microphone>();

        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.Microphone>();
        }

        if (status != PermissionStatus.Granted)
        {
            return;
        }
    }

    private async Task SetupRecognizerTwo()
    {
        var speechConfig = ConfigureSpeechTranslation();

        var (audioConfig, pushStream) = ConfigureAudioStream();

        var translationRecognizer = new TranslationRecognizer(speechConfig, audioConfig);

        RegisterRecognizers(translationRecognizer);

        await ProcessAudio(translationRecognizer, pushStream);
    }

    private async Task SetupRecognizerOne()
    {
        var speechConfig = ConfigureSpeechTranslation();

        var (audioConfig, pushStream) = ConfigureAudioStream();

        var translationRecognizer = new TranslationRecognizer(speechConfig, audioConfig);

        RegisterRecognizers(translationRecognizer);

        await ProcessAudio(translationRecognizer, pushStream);
    }

    private SpeechTranslationConfig ConfigureSpeechTranslation()
    {
        var speechTranslationConfig = SpeechTranslationConfig.FromSubscription("058a34b3c1cf40399a6a951d772ecd94", "swedencentral");

        speechTranslationConfig.SpeechRecognitionLanguage = "en-US";
        speechTranslationConfig.AddTargetLanguage("sv-SE");

        // Adjustments
        //speechTranslationConfig.SetProperty("InitialSilenceTimeout", "7000");
        //speechTranslationConfig.SetProperty("EndSilenceTimeout", "1500");
        speechTranslationConfig.OutputFormat = OutputFormat.Simple;

        return speechTranslationConfig;
    }
    private (AudioConfig, PushAudioInputStream) ConfigureAudioStream()
    {
        var pushStream = AudioInputStream.CreatePushStream(AudioStreamFormat.GetWaveFormatPCM(44100, 16, 1));
        var audioConfig = AudioConfig.FromStreamInput(pushStream);
        return (audioConfig, pushStream);
    }

    private void RegisterRecognizers(TranslationRecognizer translationRecognizer)
    {
        translationRecognizer.Recognizing += (sender, args) =>
        {
            Console.WriteLine($"RECOGNIZING: Text={args.Result.Text}");
        };

        translationRecognizer.Recognized += (sender, args) =>
        {
            Console.WriteLine($"RECOGNIZED: Text={args.Result.Translations["sv"]}");
        };

        translationRecognizer.Canceled += (sender, args) =>
        {
            Console.WriteLine($"CANCELED: Reason={args.Reason}");
            Console.WriteLine($"CANCELED: Reason={args.ErrorCode}");
            Console.WriteLine($"CANCELED: Reason={args.ErrorDetails}");
        };

        translationRecognizer.SessionStarted += (sender, args) =>
        {
            Console.WriteLine($"SessionStarted: Reason={args.SessionId}");
        };

        translationRecognizer.SessionStopped += (sender, args) =>
        {
            Console.WriteLine($"SessionStopped: Reason={args.SessionId}");
        };

        translationRecognizer.SpeechStartDetected += (sender, args) =>
        {
            Console.WriteLine($"SpeechStartDetected: Reason={args.SessionId}");
        };
    }

    private async Task ProcessAudio(TranslationRecognizer translationRecognizer, PushAudioInputStream pushStream)
    {
        int sampleRate = 44100;
        var channelConfig = ChannelIn.Mono;
        Encoding audioFormat = Encoding.Pcm16bit;

        int bufferSize = 1024;
        byte[] audioBuffer = new byte[bufferSize];
        AudioRecord audioRecord = new(AudioSource.Mic, sampleRate, channelConfig, audioFormat, bufferSize);


        bool started = true;
        while (true)
        {
            if (started)
            {
                audioRecord.StartRecording();
                await translationRecognizer.StartContinuousRecognitionAsync();
                started = false;
            }

            if (!started)
            {
                await translationRecognizer.StopContinuousRecognitionAsync();
                audioRecord.Stop();
            }

            int bytesRead = audioRecord.Read(audioBuffer, 0, audioBuffer.Length);
            if (bytesRead > 0)
            {
                pushStream.Write(audioBuffer, bytesRead);
            }
        }
    }

}

