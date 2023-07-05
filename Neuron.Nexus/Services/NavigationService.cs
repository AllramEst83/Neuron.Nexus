// Using directives for necessary namespaces
using Neuron.Nexus.Pages;

// Namespace where the navigation services are defined
namespace Neuron.Nexus.Services;
// Interface that defines a contract for a navigation service
// It declares three methods for navigating to specific pages, asynchronously
public interface INavigationService
{
    // Method that when implemented, navigates to the Speak page
    Task NavigateToSpeakPageAsync();
    // Method that when implemented, navigates to the Speech page
    Task NavigateToSpeechPageAsync();
    // Method that when implemented, navigates to the Summarize Documents page
    Task NavigateToSummarizeDocumentsPageAsync();
}

// Class that implements the INavigationService interface
public class NavigationService : INavigationService
{
    // Private readonly field for storing the injected INavigation object
    private readonly INavigation _navigation;

    // Constructor that takes an INavigation object and assigns it to the private field
    public NavigationService(INavigation navigation)
    {
        _navigation = navigation;
    }

    // Method that navigates to the Speak page by pushing it to the navigation stack
    public async Task NavigateToSpeakPageAsync()
    {
        await _navigation.PushAsync(new SpeakPage());
    }

    // Method that navigates to the Speech page by pushing it to the navigation stack
    public async Task NavigateToSpeechPageAsync()
    {
        await _navigation.PushAsync(new SpeechPage());
    }

    // Method that navigates to the Summarize Documents page by pushing it to the navigation stack
    public async Task NavigateToSummarizeDocumentsPageAsync()
    {
        await _navigation.PushAsync(new SummarizeDocumentsPage());
    }
}
