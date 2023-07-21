#if ANDROID
using Android.Media;
#endif
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Translation;
using Neuron.Nexus.Models;
using System.Collections.ObjectModel;
using OutputFormat = Microsoft.CognitiveServices.Speech.OutputFormat;


//Generera talet från texten

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
             _isLanguageOneSet = true;
            CheckAndInitializeRecognizers();

        }
    }
    public string LanguageTwoToBeSent
    {
        set
        {
            LanguageTwo = Newtonsoft.Json.JsonConvert.DeserializeObject<LanguageOption>(Uri.UnescapeDataString(value));
            _isLanguageTwoSet = true;
            CheckAndInitializeRecognizers();
        }
    }

    private bool _isLanguageOneSet = false;
    private bool _isLanguageTwoSet = false;
    [ObservableProperty]
    private LanguageOption languageOne = null;
    [ObservableProperty]
    private LanguageOption languageTwo = null;
    [ObservableProperty]
    string recognitionTextOne = "";
    [ObservableProperty]
    string recognitionTextTwo = "";
    [ObservableProperty]
    string backgroundStatus;

    private ObservableCollection<UserMessage> _userMessages;
    public ObservableCollection<UserMessage> UserMessages
    {
        get => _userMessages;
        set => SetProperty(ref _userMessages, value);
    }
    private TranslationRecognizer _translationRecognizerOne;
    private TranslationRecognizer _translationRecognizerTwo;
#if ANDROID
    private AudioRecord _audioRecord;
#endif
    private PushAudioInputStream _pushStreamOne;
    private PushAudioInputStream _pushStreamTwo;

    private bool _isRecognizerOneActive = false;
    private bool _isRecognizerTwoActive = false;
    private bool _continueProcessing = true;
    private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public SpeakPageViewModel()
    {
        UserMessages = new ObservableCollection<UserMessage>();
        SetupOnDisappearEvent();
        SetupToSleepEvent();
        SetupInitializeAfterResueEvent();
    }

    [RelayCommand]
    async Task Stop()
    {
        BackgroundStatus = "Stopped listening";

        SendAnimateButtonMessage(ButtonsEnum.StopBtn);

        if (_isRecognizerOneActive)
        {
            await _translationRecognizerOne.StopContinuousRecognitionAsync();
            _isRecognizerOneActive = false;
        }

        if (_isRecognizerTwoActive)
        {
            await _translationRecognizerTwo.StopContinuousRecognitionAsync();
            _isRecognizerTwoActive = false;
        }

#if ANDROID
        _audioRecord.Stop();
#endif

        SendChangeBorderColorMesssgae(ButtonsEnum.StopBtn);
    }

    [RelayCommand(IncludeCancelCommand = true)]
    async Task SpeakLanguageTwo(CancellationToken cancellationToken)
    {

        SendAnimateButtonMessage(ButtonsEnum.LanguageTwoBtn);

#if ANDROID
        _audioRecord.StartRecording();
#endif

        try
        {
            if (_isRecognizerOneActive)
            {
                await _translationRecognizerOne.StopContinuousRecognitionAsync();
                _isRecognizerOneActive = false;
            }

            if (!_isRecognizerTwoActive)
            {
                await _translationRecognizerTwo.StartContinuousRecognitionAsync();
                _isRecognizerTwoActive = true;
            }

            SendChangeBorderColorMesssgae(ButtonsEnum.LanguageTwoBtn);
            BackgroundStatus = $"Speak {LanguageTwo.NativeLanguageName} now.";
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    [RelayCommand(IncludeCancelCommand = true)]
    async Task SpeakLanguageOne(CancellationToken cancellationToken)
    {

        SendAnimateButtonMessage(ButtonsEnum.LanguageOneBtn);

#if ANDROID
        _audioRecord.StartRecording();
#endif

        try
        {
            if (_isRecognizerTwoActive)
            {
                await _translationRecognizerTwo.StopContinuousRecognitionAsync();
                _isRecognizerTwoActive = false;
            }

            if (!_isRecognizerOneActive)
            {
                await _translationRecognizerOne.StartContinuousRecognitionAsync();
                _isRecognizerOneActive = true;
            }

            SendChangeBorderColorMesssgae(ButtonsEnum.LanguageOneBtn);
            BackgroundStatus = $"Speak {LanguageOne.NativeLanguageName} now.";
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public void Initialize()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                await CheckForMicPermission();
                SetupRecognizerOne();
                SetupRecognizerTwo();

                if (!_continueProcessing)
                {
                    _continueProcessing = !_continueProcessing;
                }

                await ProcessAudio();
            }
            catch (Exception ex)
            {

                throw;
            }
        });

    }

    private static async Task CheckForMicPermission()
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

    private void SetupRecognizerTwo()
    {
        var speechConfig = ConfigureSpeechTranslation(LanguageTwo.FullLanguageCode, LanguageOne.FullLanguageCode);

        (var audioConfig, _pushStreamTwo) = ConfigureAudioStream();

        _translationRecognizerTwo = new TranslationRecognizer(speechConfig, audioConfig);

        RegisterRecognizers(_translationRecognizerTwo, LanguageOne.ShortLanguageCode, 2);
    }

    private void SetupRecognizerOne()
    {
        var speechConfig = ConfigureSpeechTranslation(LanguageOne.FullLanguageCode, LanguageTwo.FullLanguageCode);

        (var audioConfig, _pushStreamOne) = ConfigureAudioStream();

        _translationRecognizerOne = new TranslationRecognizer(speechConfig, audioConfig);

        RegisterRecognizers(_translationRecognizerOne, LanguageTwo.ShortLanguageCode, 1);
    }

    private SpeechTranslationConfig ConfigureSpeechTranslation(string fromLanguage, string toLanguage)
    {
        var speechTranslationConfig = SpeechTranslationConfig.FromSubscription("058a34b3c1cf40399a6a951d772ecd94", "swedencentral");

        speechTranslationConfig.SpeechRecognitionLanguage = fromLanguage;
        speechTranslationConfig.AddTargetLanguage(toLanguage);

        // Adjustments
        //speechTranslationConfig.SetProperty("InitialSilenceTimeout", "7000");
        //speechTranslationConfig.SetProperty("EndSilenceTimeout", "1500");
        speechTranslationConfig.OutputFormat = OutputFormat.Detailed;

        return speechTranslationConfig;
    }
    private (AudioConfig, PushAudioInputStream) ConfigureAudioStream()
    {
        var pushStream = AudioInputStream.CreatePushStream(AudioStreamFormat.GetWaveFormatPCM(44100, 16, 1));
        var audioConfig = AudioConfig.FromStreamInput(pushStream);
        return (audioConfig, pushStream);
    }

    private void RegisterRecognizers(TranslationRecognizer translationRecognizer, string translatedLanguageKey, int user)
    {
          translationRecognizer.SpeechStartDetected += async (sender, args) =>
          {
              BackgroundStatus = "Speech detected.";
          };

        translationRecognizer.Synthesizing += async (sender, args) =>
        {
            BackgroundStatus = "Thinking...";
        };

        translationRecognizer.Recognized += async (sender, args) =>
        {
            switch (args.Result.Reason)
            {
                case ResultReason.TranslatedSpeech:                    

                    if (args.Result.Translations.Count() > 0)
                    {
                        var translatedMessage = args.Result.Translations[translatedLanguageKey];
                        UserMessages.Add(new UserMessage()
                        {
                            User = user,
                            ChatMessage = translatedMessage
                        });

                        AddNewMessageAndScrollCollectionView(translatedMessage);

                        BackgroundStatus = "Listening...";
                    }

                    break;

                case ResultReason.Canceled:
                    await ShowToast(args.Result.Reason.ToString() ?? "Unable to recognize speech");

                    break;

                default:
                    Console.WriteLine(args.Result.Reason.ToString());
                    break;
            }
        };

        translationRecognizer.Canceled += async (sender, args) =>
        {
            await ShowToast(args.Result.Text ?? "Unable to recognize speech");
        };
    }

    private void SendChangeBorderColorMesssgae(ButtonsEnum button)
    {
        if (MainThread.IsMainThread)
        {
            WeakReferenceMessenger.Default.Send(new BorderColorMessage(button));
        }
        else
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                WeakReferenceMessenger.Default.Send(new BorderColorMessage(button));
            });
        }
    }

    private void SendAnimateButtonMessage(ButtonsEnum button)
    {

        if (MainThread.IsMainThread)
        {
            WeakReferenceMessenger.Default.Send(new AnimateButtonMessage(button));
        }
        else
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                WeakReferenceMessenger.Default.Send(new AnimateButtonMessage(button));
            });
        }
    }

    private void AddNewMessageAndScrollCollectionView(string message)
    {

        if (MainThread.IsMainThread)
        {
            WeakReferenceMessenger.Default.Send(new NewMessageMessage(message));
        }
        else
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                WeakReferenceMessenger.Default.Send(new NewMessageMessage(message));
            });
        }
    }

    private async Task ShowToast(string message)
    {
        if (MainThread.IsMainThread)
        {
            await Toast.Make(message ?? "Unable to recognize speech", ToastDuration.Long).Show(CancellationToken.None);
        }
        else
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Toast.Make(message ?? "Unable to recognize speech", ToastDuration.Long).Show(CancellationToken.None);
            });
        }

    }

    private async Task ProcessAudio()
    {
        //Audio recorder for android, IOS and 
#if ANDROID
        int sampleRate = 44100;
        var channelConfig = ChannelIn.Mono;
        Encoding audioFormat = Encoding.Pcm16bit;

        int bufferSize = 1024;
        byte[] audioBuffer = new byte[bufferSize];
        _audioRecord = new(AudioSource.Mic, sampleRate, channelConfig, audioFormat, bufferSize);
        _audioRecord.StartRecording();

        await Task.Run(async () =>
        {
            while (_continueProcessing)
            {
                await _semaphore.WaitAsync();
                try
                {
                    int bytesRead = await _audioRecord.ReadAsync(audioBuffer, 0, audioBuffer.Length);
                    if (bytesRead > 0)
                    {
                        if (_isRecognizerOneActive)
                        {
                            _pushStreamOne.Write(audioBuffer, bytesRead);
                        }
                        else
                        {
                            _pushStreamTwo.Write(audioBuffer, bytesRead);
                        }
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        });
#endif
    }

    /// <summary>
    /// For when a back button is clicked
    /// </summary>
    public void StopProcessing()
    {
        _continueProcessing = false;
#if ANDROID
        _audioRecord?.Stop();
#endif
    }

    private void DisposeOfResources()
    {
        StopProcessing();

        try { _translationRecognizerOne.Dispose(); } catch { /* Handle or log error */ }
        try { _translationRecognizerTwo.Dispose(); } catch { /* Handle or log error */ }
        try { _pushStreamOne.Dispose(); } catch { /* Handle or log error */ }
        try { _pushStreamTwo.Dispose(); } catch { /* Handle or log error */ }
#if ANDROID

        try { _audioRecord.Dispose(); } catch { /* Handle or log error */ }
#endif
    }

    private void SetupOnDisappearEvent()
    {
        WeakReferenceMessenger.Default.Register<AppDisappearingMessage>(this, (r, m) =>
        {
            DisposeOfResources();
            UnregisterMessages();
        });
    }

    private void SetupToSleepEvent()
    {
        WeakReferenceMessenger.Default.Register<OnAppToSpeepMessage>(this, (r, m) =>
        {
            DisposeOfResources();
        });
    }

    private void CheckAndInitializeRecognizers()
    {
        if (_isLanguageOneSet && _isLanguageTwoSet)
        {
            Initialize();
        }
    }

    private void SetupInitializeAfterResueEvent()
    {
        WeakReferenceMessenger.Default
            .Register<OnInitializeAfterResumMessage>(this, (r, m) =>
            {
                Initialize();
            });
    }

    private void UnregisterMessages()
    {
        WeakReferenceMessenger.Default.Unregister<AppDisappearingMessage>(this);
        WeakReferenceMessenger.Default.Unregister<OnAppToSpeepMessage>(this);
    }
}

