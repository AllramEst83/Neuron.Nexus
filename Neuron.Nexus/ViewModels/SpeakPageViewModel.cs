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
using Microsoft.Extensions.Options;
using Neuron.Nexus.Models;
using Neuron.Nexus.Services;
using Sentry;
using System.Collections.ObjectModel;
using OutputFormat = Microsoft.CognitiveServices.Speech.OutputFormat;

namespace Neuron.Nexus.ViewModels;

[QueryProperty(nameof(LanguageOneToBeSent), "languageOneToBeSent")]
[QueryProperty(nameof(LanguageTwoToBeSent), "languageTwoToBeSent")]
public partial class SpeakPageViewModel : BaseViewModel
{
    //TODO:
    //Look into if the setup and teardown of events an initilization is done correctlly
    //or if we can unsubcribe onSleep and resubcribe on onResume
    public string LanguageOneToBeSent
    {
        set
        {
            LanguageOne = Newtonsoft.Json.JsonConvert.DeserializeObject<LanguageOption>(Uri.UnescapeDataString(value));
            _isLanguageOneSet = true;
            CheckViewModelArgumenstAndInitialize();

        }
    }
    public string LanguageTwoToBeSent
    {
        set
        {
            LanguageTwo = Newtonsoft.Json.JsonConvert.DeserializeObject<LanguageOption>(Uri.UnescapeDataString(value));
            _isLanguageTwoSet = true;
            CheckViewModelArgumenstAndInitialize();
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
    public readonly IAndroidAudioRecordService androidAudioRecordService;
    private readonly IAndroidAudioPlayerService androidAudioPlayerService;
#endif
    private readonly AppSettings appSettings;
    private TranslationRecognizer _translationRecognizerOne;
    private TranslationRecognizer _translationRecognizerTwo;
    private PushAudioInputStream _pushStreamOne;
    private PushAudioInputStream _pushStreamTwo;

    private bool _isRecognizerOneActive = false;
    private bool _isRecognizerTwoActive = false;
    private bool _continueProcessing = true;
    private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public SpeakPageViewModel(
        IOptions<AppSettings> appSettings
#if ANDROID
        ,
        IAndroidAudioRecordService androidAudioRecordService,
        IAndroidAudioPlayerService androidAudioPlayerService
#endif
        )
    {
        UserMessages = new ObservableCollection<UserMessage>();
        this.appSettings = appSettings.Value;
#if ANDROID
        this.androidAudioRecordService = androidAudioRecordService;
        this.androidAudioPlayerService = androidAudioPlayerService;
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
            SentrySdk.CaptureException(ex);
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
            SentrySdk.CaptureException(ex);
            Console.Write("Error thrown when trying to SpeakLanguageTwo", ex);
            throw;
        }
    }
    [RelayCommand]
    async Task HandleFrameTapped(UserMessage messsage)
    {
#if ANDROID
        await Stop();
        UpdateUIStatustext("Playing audio");

        bool wasRecording = androidAudioRecordService.IsRecording;
        if (wasRecording)
        {
            androidAudioRecordService.StopRecording();
        }

        if (androidAudioRecordService.GetRecordState == RecordState.Stopped)
        {
            await androidAudioPlayerService.PlayAudio(messsage.ChatMessage, messsage.Language);
        }

        if (wasRecording)
        {
            androidAudioRecordService.StartRecording();
        }
# endif
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
                SentrySdk.CaptureException(ex);
                Console.Write("Error when trying to Initialize SpeakViewModel", ex);
                throw;
            }
        });

    }
    private void SetupEvents()
    {
        SetupOnDisappearEvent();
        SetupToSleepEvent();
        SetupInitializeAfterResueEvent();
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

        RegisterRecognizers(_translationRecognizerOne, LanguageTwo.ShortLanguageCode, LanguageTwo.FullLanguageCode, 1);
    }

    private void SetupRecognizerTwo()
    {
        var speechConfig = ConfigureSpeechTranslation(LanguageTwo.FullLanguageCode, LanguageOne.FullLanguageCode);

        (var audioConfig, _pushStreamTwo) = ConfigureAudioStream();

        _translationRecognizerTwo = new TranslationRecognizer(speechConfig, audioConfig);

        RegisterRecognizers(_translationRecognizerTwo, LanguageOne.ShortLanguageCode, LanguageOne.FullLanguageCode, 2);
    }
    #endregion
    #region TranslationRecognizer configuration

    private SpeechTranslationConfig ConfigureSpeechTranslation(string fromLanguage, string toLanguage)
    {
        var speechTranslationConfig = SpeechTranslationConfig.FromSubscription(appSettings.AzureKeys.AzureSubscriptionKey, appSettings.AzureKeys.AzureRegion);

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
    private void RegisterRecognizers(TranslationRecognizer translationRecognizer, string translatedLanguageKey, string fullLanguageCode, int user)
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
                            ChatMessage = translatedMessage,
                            Language = fullLanguageCode
                        });

                        AddNewMessageAndScrollCollectionView(translatedMessage);
                        UpdateUIStatustext("Listening for speeach...");
                    }

                    break;

                default:
                    var message = $"{args.Result.Reason} - {args.Result.Text}";
                    SentrySdk.CaptureMessage(message);
                    Console.WriteLine(message);
                    break;
            }
        };

        translationRecognizer.Canceled += async (sender, args) =>
        {
            string message;
            switch (args.Result.Reason)
            {
                case ResultReason.Canceled:
                    message = "Translation has stopped. Please go back and select languages again.";
                    break;
                case ResultReason.NoMatch:
                    message = "Unable to translate. Please try again. Please go back and select languages again.";
                    break;

                default:
                    message = "An unexpected error occured. Please restart the application";
                    break;

            }

            UpdateUIStatustext(message);
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
#if ANDROID
                try
                {
                    if (androidAudioRecordService != null && androidAudioRecordService.IsRecording)
                    {
                        var (audioBuffer, bytesRead) = await androidAudioRecordService.GetAudioStream();
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
                catch (Exception ex)
                {
                    SentrySdk.CaptureException(ex);
                    Console.WriteLine("Error occured while reading from the audio stream.", ex);
                }
                finally
                {
                    _semaphore.Release();
                }
#endif
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
            _isLanguageOneSet = false;
            _isLanguageTwoSet = false;
            UserMessages.Clear();
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
        WeakReferenceMessenger.Default.Unregister<OnInitializeAfterResumMessage>(this);
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

        try
        {
            _translationRecognizerOne.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occured wile trying to dispse of _translationRecognizerOne", ex);
        }

        try
        {
            _translationRecognizerTwo.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occured wile trying to dispse of _translationRecognizerTwo", ex);
        }

        try
        {
            _pushStreamOne.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occured wile trying to dispse of _pushStreamOne", ex);
        }

        try
        {
            _pushStreamTwo.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occured wile trying to dispse of _pushStreamTwo", ex);
        }

#if ANDROID
        try
        {
            if (androidAudioRecordService.IsRecording) { androidAudioRecordService.StopRecording(); };
            androidAudioRecordService.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occured wile trying to dispse of androidAudioRecordService", ex);
        }
#endif
    }
    #endregion
    #region CheckAndInitializeRecognizers
    private void CheckViewModelArgumenstAndInitialize()
    {
        if (_isLanguageOneSet && _isLanguageTwoSet)
        {
            SetupEvents();
            Initialize();
        }
    }
    #endregion
}

