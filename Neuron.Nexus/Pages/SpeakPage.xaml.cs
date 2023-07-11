using Neuron.Nexus.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Neuron.Nexus.Models;

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

        // Create a new instance of SpeakPageViewModel.
        var viewModel = new SpeakPageViewModel();

        // Set the BindingContext of the page to this ViewModel instance.
        // The BindingContext is used for data binding in the XAML.
        BindingContext = viewModel;

        // Set up the messaging for animation. This will allow the ViewModel to trigger animations in the View.
        SetupAnimationMessaging();
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
                    await AnimateButton(StopBtn);
                    break;

                case AnimationButtonsEnum.LanguageOneBtn:
                    await AnimateButton(LanguageOneBtn);
                    break;

                case AnimationButtonsEnum.LanguageTwoBtn:
                    await AnimateButton(LanguageTwoBtn);
                    break;
            }
        });
    }

    /// <summary>
    /// Animates the given ImageButton.
    /// </summary>
    /// <param name="imageButton">The ImageButton to animate.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private static async Task AnimateButton(ImageButton imageButton)
    {
        // Scale the button up slightly over a period of 100ms.
        await imageButton.ScaleTo(1.1, 100);

        // "Wiggle" the button by rotating it slightly to the right and then to the left.
        await imageButton.RotateTo(15, 50); // Rotate 15 degrees to the right over 50ms
        await imageButton.RotateTo(-15, 50); // Rotate 15 degrees to the left over 50ms
        await imageButton.RotateTo(0, 50); // Return to the original position over 50ms

        // Scale the button back to its original size over a period of 100ms.
        await imageButton.ScaleTo(1.0, 100);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // Unregister the message when the page disappears
        WeakReferenceMessenger.Default.Unregister<AnimateButtonMessage>(this);
    }

    private void LanguagePickerOne_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}

