using CommunityToolkit.Mvvm.Messaging;
using Neuron.Nexus.Models;

namespace Neuron.Nexus;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

    protected override void OnSleep()
    {
        WeakReferenceMessenger.Default.Send(new OnAppToSleepMessage());
    }

    protected override void OnResume()
    {
        WeakReferenceMessenger.Default.Send(new OnInitializeAfterResumMessage());
    }
}
