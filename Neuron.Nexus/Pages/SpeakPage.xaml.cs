﻿using Neuron.Nexus.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Neuron.Nexus.Models;
using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace Neuron.Nexus.Pages;

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

        // Set up the messaging for animation. This will allow the ViewModel to trigger animations in the View.
        SetupAnimationMessaging();
        SetupScrollToLastItem();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Initialize the ViewModel
        await ((SpeakPageViewModel)BindingContext).Initialize();

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

    private void SetupScrollToLastItem()
    {
        WeakReferenceMessenger.Default.Register<string>(this, (r, m) =>
        {
            if (m == "NewMessageAdded")
            {
                var lastItem = ChatCollectionView.ItemsSource.Cast<UserMessage>().Last();
                ChatCollectionView.ScrollTo(lastItem, position: ScrollToPosition.End, animate: true);
            }
        });
    }

    /// <summary>
    /// Sets up the animation messaging for this page.
    /// </summary>
    private void SetupAnimationMessaging()
    {
        WeakReferenceMessenger.Default.Register<AnimateButtonMessage>(this, async (r, m) =>
        {
            switch (m.ButtonName)
            {
                case AnimationButtonsEnum.StopBtn:
                    //await AnimateButton(StopBtn);
                    break;

                case AnimationButtonsEnum.LanguageOneBtn:
                    await AnimateButton(LanguageOneBtn, LanguageOneName);
                    break;

                case AnimationButtonsEnum.LanguageTwoBtn:
                    await AnimateButton(LanguageTwoBtn, LanguageTwoName);
                    break;
            }
        });
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

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // Unregister the message when the page disappears
        WeakReferenceMessenger.Default.Unregister<AnimateButtonMessage>(this);
        WeakReferenceMessenger.Default.Unregister<string>(this);
    }

    private void LanguagePickerOne_SelectedIndexChanged(object sender, EventArgs e)
    {

    }


}

