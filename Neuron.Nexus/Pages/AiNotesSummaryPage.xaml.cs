using CommunityToolkit.Mvvm.Messaging;
using Neuron.Nexus.Models;
using Neuron.Nexus.ViewModels;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Neuron.Nexus.Pages;

public partial class AiNotesSummaryPage : ContentPage
{
    private AiNotesSummaryViewModel viewModel;

    public AiNotesSummaryPage()
    {
        InitializeComponent();
        viewModel = new AiNotesSummaryViewModel();
        BindingContext = viewModel;

        RegisterEvents();
    }

    void RegisterEvents()
    {
        WeakReferenceMessenger.Default.Register<OpenModalMessage>(this, async (r, m) =>
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Navigation.PushModalAsync(new SummaryPreviewPage(m.MarkdownText));
            });
        });
    }

    void UnregisterEvents()
    {
        WeakReferenceMessenger.Default.Unregister<OpenModalMessage>(this);
    }

    void onDisappearing(object sender, EventArgs e)
    {
        UnregisterEvents();
    }
}