
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Media;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Neuron.Nexus.Models;
using Neuron.Nexus.Pages;
using Neuron.Nexus.Repositories;
using Neuron.Nexus.Services;
using Neuron.Nexus.ViewModels;
using System.Reflection;

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

        using (Stream stream = assembly.GetManifestResourceStream("Neuron.Nexus.appsettings.json"))
        {
            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            builder.Configuration.AddConfiguration(config);
        }

        builder.Services.AddOptions<AppSettings>()
                .Bind(builder.Configuration.GetSection("AzureKeys"));

        builder.Services

            //Services
            .AddSingleton<ISpeechToText>(SpeechToText.Default)
            .AddSingleton<ILanguageRepository, LanguageRepository>()
            .AddSingleton<ILanguageService, LanguageService>()
#if ANDROID
           .AddSingleton<IAndroidAudioRecordService, AndroidAudioRecordService>()
#endif
#if IOS
           .AddTransient<IOSAudioRecorderService, IOSAudioRecorderService>()
#endif
           //Pages
           .AddSingleton<MainPage>()
           .AddSingleton<SpeakPage>()
           .AddSingleton<SpeechPage>()
           .AddSingleton<SelectLanguagePage>()

           //ViewModels
           .AddSingleton<MainPageViewModel>()
           .AddSingleton<SpeakPageViewModel>()
           .AddSingleton<SelectLanguagePageViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
