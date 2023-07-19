using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Media;
using Microsoft.Extensions.Configuration;
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

        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("Neuron.Nexus.appsettings.json");

        var config = new ConfigurationBuilder()
                 .AddJsonStream(stream)
                 .Build();

        builder.Configuration.AddConfiguration(config);


        builder.Services
            .AddSingleton<ISpeechToText>(SpeechToText.Default)
        //LanguageRepository
        .AddTransient<ILanguageRepository, LanguageRepository>()
               //SpeakService
           .AddTransient<ILanguageService, LanguageService>()
           //MainPage
           .AddTransient<MainPage>()
           .AddTransient<MainPageViewModel>()
           //SpeakPage
           .AddTransient<SpeakPage>()
           .AddTransient<SpeakPageViewModel>()
           //SpeechPage
           .AddTransient<SpeechPage>()
           //SelectLanguagePage
           .AddTransient<SelectLanguagePage>()
           .AddTransient<SelectLanguagePageViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
