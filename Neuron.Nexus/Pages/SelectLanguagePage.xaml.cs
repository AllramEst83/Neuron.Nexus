using Neuron.Nexus.Services;
using Neuron.Nexus.ViewModels;

namespace Neuron.Nexus.Pages;

public partial class SelectLanguagePage : ContentPage
{
    public SelectLanguagePage()
	{
        InitializeComponent();

        // Create a new instance of SelectLanguagePage with dependency injetced services.
        var viewModel = Application.Current.Handler.MauiContext.Services.GetService<SelectLanguagePageViewModel>();

        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Initialize the ViewModel
        await ((SelectLanguagePageViewModel)BindingContext).Initialize();
    }
}