
using CommunityToolkit.Maui.Alerts;
using Neuron.Nexus.Services;
using Neuron.Nexus.ViewModels;

namespace Neuron.Nexus.Pages;

public partial class SelectLanguagePage : ContentPage
{
    private readonly IConnectivityService connectivityService;
        public SelectLanguagePage(IConnectivityService connectivityService)
    {
        InitializeComponent();

        // Create a new instance of SelectLanguagePage with dependency injetced services.
        var viewModel = Application.Current.Handler.MauiContext.Services.GetService<SelectLanguagePageViewModel>();

        BindingContext = viewModel;
        this.connectivityService = connectivityService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        bool isConnected = connectivityService.IsConnected();
        if (!isConnected)
        {
            Toast.Make("No internet connection! Please connect to the internet.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
        }
        else
        {
            ((SelectLanguagePageViewModel)BindingContext).Initialize();
        }

        Connectivity.ConnectivityChanged += OnConnectivityChanged;
    }

    protected override void OnDisappearing()
    {
        Connectivity.ConnectivityChanged -= OnConnectivityChanged;
        base.OnDisappearing();
    }

    private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        if (e.NetworkAccess == NetworkAccess.Internet)
        {
            Toast.Make("Application is connected. Thank you!", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
            
            //Change to using WeakRef messaging
            ((SelectLanguagePageViewModel)BindingContext).Initialize();
        }
        else
        {
            DisplayAlert("No internet access", "The application does not have a internet connection. Please make shure you are connected to the internet.", "Ok");
        }
    }

}