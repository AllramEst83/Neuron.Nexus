using CommunityToolkit.Mvvm.Messaging;
using Neuron.Nexus.Models;
using Neuron.Nexus.ViewModels;
using System.Windows.Input;

namespace Neuron.Nexus.Pages;

public partial class SummaryPreviewPage : ContentPage
{
	private readonly SummaryPreviewViewModel viewModel;
    public SummaryPreviewPage(string encodedMarkdownText)
	{
		InitializeComponent();

        viewModel = new SummaryPreviewViewModel(encodedMarkdownText);

		BindingContext = viewModel;

        RegisterEvents();
    }

    void RegisterEvents()
    {
        WeakReferenceMessenger.Default.Register<CloseModalMessage>(this, async (r, m) =>
        {
            await Navigation.PopModalAsync();
        });
    }

    void onDisappearing(object sender, EventArgs e)
    {
        WeakReferenceMessenger.Default.Unregister<CloseModalMessage>(this);
    }
}