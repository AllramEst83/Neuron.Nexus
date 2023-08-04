using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.Messaging;
using Neuron.Nexus.Managers;
using Neuron.Nexus.Models;
using Neuron.Nexus.Pages;
using Neuron.Nexus.Resources.Languages;
using System.Globalization;

namespace Neuron.Nexus
{
    public partial class App : Application
    {
        private const string SelectLanguagePageRoute = "//SelectLanguagePage";
        private const string NoInternetAccessTitle = "No internet access";
        private const string NoInternetAccessMessage = "Oops! We've lost the internet connection. We'll take you back to the start page for now. Please check your connection and try again.";
        private const string OkButtonText = "Ok";
        private const string ToastMessageNoInternet = "No internet connection! Please connect to the internet.";
        private const string ToastMessageInternetIsBack = "Application is connected. Thank you!";

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            SetApplangugaes();
            
            SubscribeToEvents();
        }

        protected override void OnSleep()
        {
            UnsubscribeFromEvents();
            WeakReferenceMessenger.Default.Send(new OnAppToSleepMessage());
        }

        protected override void OnResume()
        {
            SubscribeToEvents();
            WeakReferenceMessenger.Default.Send(new OnInitializeAfterResumMessage());
        }

        private void SubscribeToEvents()
        {
            UnsubscribeFromEvents();
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
        }

        private void UnsubscribeFromEvents()
        {
            Connectivity.ConnectivityChanged -= OnConnectivityChanged;
        }

        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            HandleConnectivityChange(e).ConfigureAwait(false);
        }
        private async Task HandleConnectivityChange(ConnectivityChangedEventArgs e)
        {
            var currentPage = (Shell.Current.CurrentItem.CurrentItem as IShellSectionController)?.PresentedPage;

            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                if (currentPage is SpeakPage)
                {
                    await MainPage.DisplayAlert(NoInternetAccessTitle, NoInternetAccessMessage, OkButtonText);
                    await Shell.Current.GoToAsync(SelectLanguagePageRoute);
                    return;
                }

                if (currentPage is not SelectLanguagePage)
                {
                    await ShowToast(ToastMessageNoInternet);
                }
                return;
            }

            if (currentPage is not SelectLanguagePage && currentPage is not SpeakPage)
            {
                await ShowToast(ToastMessageInternetIsBack);
            }
        }

        private async Task ShowToast(string message)
        {
            await Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
        }


        private void SetApplangugaes()
        {
            string currentCulture = Preferences.Get("currentCulture", "en-US");

            LocalizationResourceManager.Instance.SetCulture(new CultureInfo(currentCulture));

            Preferences.Set("currentCulture", currentCulture);
        }
    }
}
