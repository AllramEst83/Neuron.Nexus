using Neuron.Nexus.ViewModels;

namespace Neuron.Nexus.Pages;

public partial class AiNotesPage : ContentPage
{
    private readonly AiNotesViewModel viewModel;

    public AiNotesPage()
	{
		InitializeComponent();

        viewModel = Application.Current.Handler.MauiContext.Services.GetService<AiNotesViewModel>();

        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
       viewModel.Initiate();
    }

    protected override void OnDisappearing()
    {
        viewModel.DisposeOfResources();
    }
}