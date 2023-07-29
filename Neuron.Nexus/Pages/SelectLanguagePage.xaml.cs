
using CommunityToolkit.Maui.Alerts;
using Neuron.Nexus.Services;
using Neuron.Nexus.ViewModels;

namespace Neuron.Nexus.Pages;

public partial class SelectLanguagePage : ContentPage
{
    private readonly IConnectivityService connectivityService;
    private readonly IUserPersmissionsService userPersmissionsService;

    public SelectLanguagePage(IConnectivityService connectivityService, IUserPersmissionsService userPersmissionsService)
    {
        InitializeComponent();

        // Create a new instance of SelectLanguagePage with dependency injetced services.
        var viewModel = Application.Current.Handler.MauiContext.Services.GetService<SelectLanguagePageViewModel>();

        BindingContext = viewModel;
        this.connectivityService = connectivityService;
        this.userPersmissionsService = userPersmissionsService;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        var cancellactionTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellactionTokenSource.Token;

       await userPersmissionsService.GetPermissionsFromUser(cancellationToken);

        bool isConnected = connectivityService.IsConnected();
        if (!isConnected)
        {
          await  Toast.Make("No internet connection! Please connect to the internet.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
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