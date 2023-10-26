using Neuron.Nexus.ViewModels;

namespace Neuron.Nexus.Pages;

public partial class AboutPage : ContentPage
{
    public AboutPage()
    {
        InitializeComponent();

        var viewModel = Application.Current.Handler.MauiContext.Services.GetService<AboutPageViewModel>();

        BindingContext = viewModel;
    }
}