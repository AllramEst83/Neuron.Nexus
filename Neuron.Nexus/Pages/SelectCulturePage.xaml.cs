using Neuron.Nexus.ViewModels;

namespace Neuron.Nexus.Pages;

public partial class SelectCulturePage : ContentPage
{
    public SelectCulturePage()
    {
        InitializeComponent();

        var viewModel = Application.Current.Handler.MauiContext.Services.GetService<SelectCulturePageViewModel>();
        BindingContext = viewModel;

    }
}