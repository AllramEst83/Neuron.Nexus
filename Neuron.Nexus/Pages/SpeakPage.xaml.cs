using Neuron.Nexus.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Neuron.Nexus.Models;
using CommunityToolkit.Maui.Alerts;
using System.Windows.Input;

namespace Neuron.Nexus.Pages;

//Initiate all when the Langugaes are set
//Dispose everything when OnDisappera
//Have a back button
//After changing app, have the app reset to langugae picker page

public partial class SpeakPage : ContentPage
{
    /// <summary>
    /// Constructor for the SpeakPage.
    /// </summary>
    public SpeakPage()
    {
        //Initialize the components (defined in XAML) of the page.
        InitializeComponent();

        // Create a new instance of SpeakPageViewModel with dependency injetced services.
        var viewModel = Application.Current.Handler.MauiContext.Services.GetService<SpeakPageViewModel>();

        // Set the BindingContext of the page to this ViewModel instance.
        // The BindingContext is used for data binding in the XAML.
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        Connectivity.ConnectivityChanged += OnConnectivityChanged;

        RegisterEvents();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        Connectivity.ConnectivityChanged += OnConnectivityChanged;

        UnRegisterEvents();
    }
    private async void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        if (e.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("No internet access", "Oops! We've lost the internet connection. We'll take you back to the start page for now. Please check your connection and try again.", "Ok");

            await Shell.Current.GoToAsync($"//{nameof(SelectLanguagePage)}");
        }
    }
    private void RegisterEvents()
    {
        SetupAnimationMessaging();
        SetupScrollToLastItemMessaging();
        UpdateImageButtonsBorderColor();
    }

    private void UnRegisterEvents()
    {
        WeakReferenceMessenger.Default.Send(new AppDisappearingMessage("App disappearing"));

        // Unregister the message when the page disappears
        WeakReferenceMessenger.Default.Unregister<AnimateButtonMessage>(this);
        WeakReferenceMessenger.Default.Unregister<NewMessageMessage>(this);
        WeakReferenceMessenger.Default.Unregister<BorderColorMessage>(this);
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (width != -1 && height != -1)
        {
            double scrollViewHeight = ChatScrollView.Height;

            ChatCollectionView.HeightRequest = scrollViewHeight;
        }
    }

    private void SetupScrollToLastItemMessaging()
    {
        WeakReferenceMessenger.Default.Register<NewMessageMessage>(this, (r, m) =>
        {
            var lastItem = ChatCollectionView.ItemsSource.Cast<UserMessage>().Last();
            ChatCollectionView.ScrollTo(lastItem, position: ScrollToPosition.End, animate: true);
        });
    }

    private void SetupAnimationMessaging()
    {
        WeakReferenceMessenger.Default.Register<AnimateButtonMessage>(this, async (r, m) =>
        {
            switch (m.ButtonName)
            {
                case ButtonsEnum.StopBtn:
                    await AnimateButton(StopBtn, StopName);
                    break;

                case ButtonsEnum.LanguageOneBtn:
                    await AnimateButton(LanguageOneBtn, LanguageOneName);
                    break;

                case ButtonsEnum.LanguageTwoBtn:
                    await AnimateButton(LanguageTwoBtn, LanguageTwoName);
                    break;
            }
        });
    }
    private void UpdateImageButtonsBorderColor()
    {
        WeakReferenceMessenger.Default.Register<BorderColorMessage>(this, (r, m) =>
        {
            if (Application.Current.Resources.TryGetValue("Primary", out var objOne) && objOne is Color primaryColorValue &&
            Application.Current.Resources.TryGetValue("Secondary", out var objTwo) && objTwo is Color secondaryColorValue)
            {
                var stopButtonBackgroundColor = StopBtn.BackgroundColor;
                var LangugaeOneBackgroundColor = LanguageOneBtn.BackgroundColor;
                var LangugaeTwoBackgroundColor = LanguageTwoBtn.BackgroundColor;

                switch (m.Button)
                {
                    case ButtonsEnum.StopBtn:

                        SetBackgroundColorIfDifferent(StopBtn, stopButtonBackgroundColor, primaryColorValue);
                        SetBackgroundColorIfDifferent(LanguageOneBtn, LangugaeOneBackgroundColor, secondaryColorValue);
                        SetBackgroundColorIfDifferent(LanguageTwoBtn, LangugaeTwoBackgroundColor, secondaryColorValue);

                        break;

                    case ButtonsEnum.LanguageOneBtn:

                        SetBackgroundColorIfDifferent(StopBtn, stopButtonBackgroundColor, secondaryColorValue);
                        SetBackgroundColorIfDifferent(LanguageOneBtn, LangugaeOneBackgroundColor, primaryColorValue);
                        SetBackgroundColorIfDifferent(LanguageTwoBtn, LangugaeTwoBackgroundColor, secondaryColorValue);
                        break;

                    case ButtonsEnum.LanguageTwoBtn:

                        SetBackgroundColorIfDifferent(StopBtn, stopButtonBackgroundColor, secondaryColorValue);
                        SetBackgroundColorIfDifferent(LanguageOneBtn, LangugaeOneBackgroundColor, secondaryColorValue);
                        SetBackgroundColorIfDifferent(LanguageTwoBtn, LangugaeTwoBackgroundColor, primaryColorValue);
                        break;
                }
            }
        });
    }

    private void SetBackgroundColorIfDifferent(ImageButton button, Color currentColor, Color targetColor)
    {
        if (currentColor != targetColor)
        {
            button.BackgroundColor = targetColor;
        }
    }

    /// <summary>
    /// Animates the given ImageButton.
    /// </summary>
    /// <param name="imageButton">The ImageButton to animate.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private static async Task AnimateButton(ImageButton imageButton, Label label)
    {
        // Scale the button up slightly over a period of 100ms.
        await imageButton.ScaleTo(1.1, 50);
        await label.ScaleTo(1.1, 50);

        // "Wiggle" the button by rotating it slightly to the right and then to the left.
        await imageButton.RotateTo(15, 20); // Rotate 15 degrees to the right over 50ms
        await label.RotateTo(15, 20); // Rotate 15 degrees to the right over 50ms
        await imageButton.RotateTo(-15, 20); // Rotate 15 degrees to the left over 50ms
        await label.RotateTo(-15, 20); // Rotate 15 degrees to the left over 50ms
        await imageButton.RotateTo(0, 20); // Return to the original position over 50ms
        await label.RotateTo(0, 20); // Return to the original position over 50ms


        // Scale the button back to its original size over a period of 100ms.
        await imageButton.ScaleTo(1.0, 50);
        await label.ScaleTo(1.0, 50);
    }

    private void LanguagePickerOne_SelectedIndexChanged(object sender, EventArgs e)
    {

    }


}

