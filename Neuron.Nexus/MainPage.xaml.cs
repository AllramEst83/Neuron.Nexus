using Neuron.Nexus.Pages;

namespace Neuron.Nexus;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

    async void OnNavigateToSTS(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SpeechToSpeechPage());
    }
}

