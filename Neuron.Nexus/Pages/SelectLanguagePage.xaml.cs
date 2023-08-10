
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.Messaging;
using Neuron.Nexus.Models;
using Neuron.Nexus.Services;
using Neuron.Nexus.ViewModels;

namespace Neuron.Nexus.Pages;

public partial class SelectLanguagePage : ContentPage
{
    private readonly IConnectivityService connectivityService;
    private readonly IUserPersmissionsService userPersmissionsService;
    private SelectLanguagePageViewModel viewModel;

    public SelectLanguagePage(IConnectivityService connectivityService, IUserPersmissionsService userPersmissionsService)
    {
        InitializeComponent();

        // Create a new instance of SelectLanguagePage with dependency injetced services.
        viewModel = Application.Current.Handler.MauiContext.Services.GetService<SelectLanguagePageViewModel>();

        BindingContext = viewModel;
        this.connectivityService = connectivityService;
        this.userPersmissionsService = userPersmissionsService;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        viewModel.SubscribeToEvents();

        WeakReferenceMessenger.Default.Send(new InitializeStartMessage());

        SubscribeToEvents();
 
        var cancellactionTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellactionTokenSource.Token;

        await userPersmissionsService.GetPermissionsFromUser(cancellationToken);

        bool isConnected = connectivityService.IsConnected();
        if (!isConnected)
        {
            await Toast.Make("No internet connection! Please connect to the internet.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
        }
        else
        {
            WeakReferenceMessenger.Default.Send(new InitializeStartMessage());
        }
    }

    protected override void OnDisappearing()
    {
        UnSubscribeToEvents();

        WeakReferenceMessenger.Default.Send(new DisposeStartMessage());


        base.OnDisappearing();
    }

    private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        if (e.NetworkAccess == NetworkAccess.Internet)
        {
            Toast.Make("Application is connected. Thank you!", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);

            WeakReferenceMessenger.Default.Send(new InitializeStartMessage());
        }
        else
        {
            DisplayAlert("No internet access", "The application does not have a internet connection. Please make shure you are connected to the internet.", "Ok");
        }
    }

    private void SubscribeToEvents()
    {
        Connectivity.ConnectivityChanged += OnConnectivityChanged;
    }

    private void UnSubscribeToEvents()
    {
        Connectivity.ConnectivityChanged -= OnConnectivityChanged;
    }
}