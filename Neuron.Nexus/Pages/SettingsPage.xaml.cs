using Neuron.Nexus.ViewModels;

namespace Neuron.Nexus.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();

        var viewModel = Application.Current.Handler.MauiContext.Services.GetService<SettingsPageViewModel>();
        BindingContext = viewModel;

    }
}