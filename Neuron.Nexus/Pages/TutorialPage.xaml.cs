using Neuron.Nexus.ViewModels;

namespace Neuron.Nexus.Pages;

public partial class TutorialPage : ContentPage
{
    public TutorialPage()
    {
        InitializeComponent();

        var viewModel = Application.Current.Handler.MauiContext.Services.GetService<TutorialPageViewModel>();

        BindingContext = viewModel;
    }
}