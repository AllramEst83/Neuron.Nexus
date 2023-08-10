using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Media;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Neuron.Nexus.Models;
using Neuron.Nexus.Pages;
using Neuron.Nexus.Repositories;
using Neuron.Nexus.Services;
using Neuron.Nexus.ViewModels;

namespace Neuron.Nexus;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        var assembly = typeof(MauiProgram).Assembly;
        string environmentName;

#if DEBUG
        environmentName = "Local";
#else
    environmentName = "";      
#endif

        string configFileName = string.IsNullOrEmpty(environmentName)
                                ? "Neuron.Nexus.appsettings.json"
                                : $"Neuron.Nexus.appsettings.{environmentName}.json";

        using (Stream stream = assembly.GetManifestResourceStream(configFileName))
        {
            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            builder.Configuration.AddConfiguration(config);
        }

        builder.Services.AddOptions<AppSettings>()
                .Bind(builder.Configuration.GetSection("ApplicationSettings"));

        builder.Services

            //Services
            .AddSingleton<IShareLogService, ShareLogService>()
            .AddSingleton<ISpeechToText>(SpeechToText.Default)
            .AddSingleton<ISpeechSynthesizerService, SpeechSynthesizerService>()
            .AddSingleton<ILanguageRepository, LanguageRepository>()
            .AddSingleton<ILanguageService, LanguageService>()
            .AddSingleton<IConnectivityService, ConnectivityService>()
            .AddSingleton<IUserPersmissionsService, UserPersmissionsService>()
#if ANDROID
            .AddSingleton<IAndroidAudioPlayerService, AndroidAudioPlayerService>()
#endif
#if ANDROID
           .AddSingleton<IAndroidAudioRecordService, AndroidAudioRecordService>()
#endif
#if IOS
           .AddTransient<IOSAudioRecorderService, IOSAudioRecorderService>()
#endif
           //Pages
           .AddSingleton<MainPage>()
           .AddSingleton<SpeakPage>()
           .AddSingleton<SelectLanguagePage>()
           .AddSingleton<SettingsPage>()
           .AddSingleton<AboutPage>()
           .AddSingleton<TutorialPage>()
           .AddSingleton<SelectCulturePage>()

           //ViewModels
           .AddSingleton<MainPageViewModel>()
           .AddSingleton<SpeakPageViewModel>()
           .AddSingleton<SelectLanguagePageViewModel>()
           .AddSingleton<SettingsPageViewModel>()
           .AddSingleton<AboutPageViewModel>()
           .AddSingleton<TutorialPageViewModel>()
           .AddSingleton<SelectCulturePageViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
