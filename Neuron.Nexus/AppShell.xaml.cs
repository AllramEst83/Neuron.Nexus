using Neuron.Nexus.Pages;

namespace Neuron.Nexus;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        // Register the routes for other pages
        Routing.RegisterRoute("SummarizeDocumentsPage", typeof(SummarizeDocumentsPage));
        Routing.RegisterRoute("SelectLanguagePage", typeof(SelectLanguagePage));
        Routing.RegisterRoute("SpeechPage", typeof(SpeechPage));
        Routing.RegisterRoute("SpeakPage", typeof(SpeakPage));
    }
}
