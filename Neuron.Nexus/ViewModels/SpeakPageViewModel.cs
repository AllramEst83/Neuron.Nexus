using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Translation;
using Microsoft.Extensions.Options;
using Neuron.Nexus.Models;
using Neuron.Nexus.Services;
using System.Collections.ObjectModel;
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
    string uIStatustext;
    private ObservableCollection<UserMessage> _userMessages;
    public ObservableCollection<UserMessage> UserMessages
    {
        get => _userMessages;
        set => SetProperty(ref _userMessages, value);
    }
#if ANDROID
    public IAndroidAudioRecordService androidAudioRecordService { get; }
#endif
    public IOptions<AppSettings> appSettings { get; }
    private TranslationRecognizer _translationRecognizerOne;
    private TranslationRecognizer _translationRecognizerTwo;
    private PushAudioInputStream _pushStreamOne;
    private PushAudioInputStream _pushStreamTwo;

    private bool _isRecognizerOneActive = false;
    private bool _isRecognizerTwoActive = false;
    private bool _continueProcessing = true;
    private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public SpeakPageViewModel(
#if ANDROID
        IAndroidAudioRecordService androidAudioRecordService,
        IOptions<AppSettings> appSettings
#endif
        )
    {
        UserMessages = new ObservableCollection<UserMessage>();
        SetupOnDisappearEvent();
        SetupToSleepEvent();
        SetupInitializeAfterResueEvent();
#if ANDROID
        this.androidAudioRecordService = androidAudioRecordService;
        this.appSettings = appSettings;

#endif
    }

    #region Commands
    [RelayCommand]
    async Task Stop()
    {
        SendAnimateButtonMessage(ButtonsEnum.StopBtn);
        StopRecording();
        UpdateUIStatustext("Stopped listening");
        await StopRecognizers();
        SendChangeBorderColorMesssgae(ButtonsEnum.StopBtn);
    }

    [RelayCommand(IncludeCancelCommand = true)]
    async Task SpeakLanguageOne(CancellationToken cancellationToken)
    {

        SendAnimateButtonMessage(ButtonsEnum.LanguageOneBtn);

        try
        {
            await StartRecognizerOne();
            StartRecording();
            SendChangeBorderColorMesssgae(ButtonsEnum.LanguageOneBtn);
            UpdateUIStatustext($"Speak {LanguageOne.NativeLanguageName} now.");
        }
        catch (Exception ex)
        {
            Console.Write("Error thrown when trying to SpeakLanguageOne", ex);
            throw;
        }
    }

    [RelayCommand(IncludeCancelCommand = true)]
    async Task SpeakLanguageTwo(CancellationToken cancellationToken)
    {
        SendAnimateButtonMessage(ButtonsEnum.LanguageTwoBtn);

        try
        {
            await StartRecognizerTwo();
            StartRecording();
            SendChangeBorderColorMesssgae(ButtonsEnum.LanguageTwoBtn);
            UpdateUIStatustext($"Speak {LanguageTwo.NativeLanguageName} now.");
        }
        catch (Exception ex)
        {
            Console.Write("Error thrown when trying to SpeakLanguageTwo", ex);
            throw;
        }
    }
    #endregion
    #region SpeakViewModel Initialize
    public void Initialize()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                await CheckForMicPermission();
                SetupRecognizerOne();
                SetupRecognizerTwo();
                await StartToProcessAudio();
            }
            catch (Exception ex)
            {
                Console.Write("Error thrown when trying to Initialize SpeakViewModel", ex);
                throw;
            }
        });

    }
    #endregion
    #region Start and stop recognizers
    private async Task StartRecognizerOne()
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
    }

    private async Task StartRecognizerTwo()
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
    }

    async Task StopRecognizers()
    {
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
    }
    #endregion
    #region Mic permission
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
    #endregion
    #region Start and stop AudioRecord
    private void StartRecording()
    {
#if ANDROID
        if (!androidAudioRecordService.IsRecording)
        {
            androidAudioRecordService.StartRecording();
        }
#endif
    }

    private void StopRecording()
    {
#if ANDROID
        if (androidAudioRecordService.IsRecording)
        {
            androidAudioRecordService.StopRecording();
        }
#endif
    }
    #endregion
    #region UI status 
    private void UpdateUIStatustext(string text)
    {
        UIStatustext = text;
    }
    #endregion
    #region Setup recognizers
    private void SetupRecognizerOne()
    {
        var speechConfig = ConfigureSpeechTranslation(LanguageOne.FullLanguageCode, LanguageTwo.FullLanguageCode);

        (var audioConfig, _pushStreamOne) = ConfigureAudioStream();

        _translationRecognizerOne = new TranslationRecognizer(speechConfig, audioConfig);

        RegisterRecognizers(_translationRecognizerOne, LanguageTwo.ShortLanguageCode, 1);
    }

    private void SetupRecognizerTwo()
    {
        var speechConfig = ConfigureSpeechTranslation(LanguageTwo.FullLanguageCode, LanguageOne.FullLanguageCode);

        (var audioConfig, _pushStreamTwo) = ConfigureAudioStream();

        _translationRecognizerTwo = new TranslationRecognizer(speechConfig, audioConfig);

        RegisterRecognizers(_translationRecognizerTwo, LanguageOne.ShortLanguageCode, 2);
    }
    #endregion
    #region TranslationRecognizer configuration

    private SpeechTranslationConfig ConfigureSpeechTranslation(string fromLanguage, string toLanguage)
    {
        var speechTranslationConfig = SpeechTranslationConfig.FromSubscription(appSettings.Value.AzureSubscriptionKey, appSettings.Value.AzureRegion);

        speechTranslationConfig.SpeechRecognitionLanguage = fromLanguage;
        speechTranslationConfig.AddTargetLanguage(toLanguage);

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
    #endregion
    #region Register recognizer events
    private void RegisterRecognizers(TranslationRecognizer translationRecognizer, string translatedLanguageKey, int user)
    {
        translationRecognizer.SpeechStartDetected += (sender, args) =>
        {
            UpdateUIStatustext("Speech detected.");
        };

        translationRecognizer.Synthesizing += (sender, args) =>
        {
            UpdateUIStatustext("Thinking...");
        };

        translationRecognizer.Recognized += async (sender, args) =>
        {
            switch (args.Result.Reason)
            {
                case ResultReason.TranslatedSpeech:

                    if (args.Result.Translations.Count > 0)
                    {
                        var translatedMessage = args.Result.Translations[translatedLanguageKey];
                        UserMessages.Add(new UserMessage()
                        {
                            User = user,
                            ChatMessage = translatedMessage
                        });

                        SpeakPageViewModel.AddNewMessageAndScrollCollectionView(translatedMessage);

                        UpdateUIStatustext("Waiting for speeach...");
                    }

                    break;

                case ResultReason.Canceled:
                    await SpeakPageViewModel.ShowToast(args.Result.Reason.ToString() ?? "Unable to recognize speech");

                    break;

                default:
                    Console.WriteLine(args.Result.Reason.ToString());
                    break;
            }
        };

        translationRecognizer.Canceled += async (sender, args) =>
        {
            await SpeakPageViewModel.ShowToast(args.Result.Text ?? "Unable to recognize speech");
        };
    }
    #endregion
    #region Processing of audio stream
    private async Task StartToProcessAudio()
    {
        //https://www.syncfusion.com/blogs/post/building-an-audio-recorder-and-player-app-in-net-maui.aspx
        //Audio recorder for android, IOS and

        if (!_continueProcessing)
        {
            _continueProcessing = !_continueProcessing;
        }

        await Task.Run(async () =>
        {
            while (_continueProcessing)
            {
                await _semaphore.WaitAsync();
                try
                {
#if ANDROID
                    if (androidAudioRecordService != null && androidAudioRecordService.IsRecording)
                    {
                        var (audioBuffer, bytesRead) = await androidAudioRecordService.GetAudioStream();
#endif
                        if (bytesRead > 0)
                        {
                            if (_isRecognizerOneActive)
                            {
                                _pushStreamOne.Write(audioBuffer, bytesRead);
                            }

                            if (_isRecognizerTwoActive)
                            {
                                _pushStreamTwo.Write(audioBuffer, bytesRead);
                            }
                        }
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        });
    }
    #endregion
    #region Event between MVVM setup
    private static void SendChangeBorderColorMesssgae(ButtonsEnum button)
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

    private static void SendAnimateButtonMessage(ButtonsEnum button)
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

    private static void AddNewMessageAndScrollCollectionView(string message)
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
        WeakReferenceMessenger.Default.Register<OnAppToSleepMessage>(this, (r, m) =>
        {
            DisposeOfResources();
        });
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
        WeakReferenceMessenger.Default.Unregister<OnAppToSleepMessage>(this);
    }
    #endregion
    #region Show toast
    private static async Task ShowToast(string message)
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
    #endregion
    #region Setup DisposeOfResources

    private void DisposeOfResources()
    {
        _continueProcessing = false;
        _isRecognizerOneActive = false;
        _isRecognizerTwoActive = false;
        UIStatustext = "";

        SendChangeBorderColorMesssgae(ButtonsEnum.StopBtn);

        try { _translationRecognizerOne.Dispose(); } catch { /* Handle or log error */ }
        try { _translationRecognizerTwo.Dispose(); } catch { /* Handle or log error */ }
        try { _pushStreamOne.Dispose(); } catch { /* Handle or log error */ }
        try { _pushStreamTwo.Dispose(); } catch { /* Handle or log error */ }

#if ANDROID
        try
        {
            if (androidAudioRecordService.IsRecording) { androidAudioRecordService.StopRecording(); };
            androidAudioRecordService.Dispose();
        }
        catch { /* Handle or log error */ }
#endif
    }
    #endregion
    #region CheckAndInitializeRecognizers
    private void CheckAndInitializeRecognizers()
    {
        if (_isLanguageOneSet && _isLanguageTwoSet)
        {
            Initialize();
        }
    }
    #endregion
}

