using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Neuron.Nexus.Pages;
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

        builder.Services
            //MainPage
           .AddTransient<MainPage>()
           .AddTransient<MainPageViewModel>()
           //SpeakkPage
           .AddTransient<SpeakPage>()
           //SpeechPage
           .AddTransient<SpeechPage>()
           .AddSingleton<INavigationService, NavigationService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
