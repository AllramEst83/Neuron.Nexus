namespace Neuron.Nexus.Services;

// The INavigationService interface defines the contract for a service that provides navigation functionality
public interface INavigationService
{
    // The NavigateToPageAsync method is a generic method that asynchronously navigates to a page of a specified type
    // The type parameter T represents the type of the page to navigate to
    // The constraint where T : Page, new() ensures that T is a subclass of Page and has a parameterless constructor
    // This method returns a Task that represents the asynchronous operation
    Task NavigateToPageAsync<T>() where T : Page, new();
}


// The NavigationService class that implements the INavigationService interface
public class NavigationService : INavigationService
{
    // A private readonly field that stores the INavigation object injected via the constructor
    private readonly INavigation _navigation;

    // The constructor of the NavigationService class that accepts an INavigation object
    // This constructor is used to inject the INavigation object at runtime
    public NavigationService(INavigation navigation)
    {
        // Assign the provided INavigation object to the private readonly field
        _navigation = navigation;
    }

    // A generic method for navigating to a specified page
    // The method takes a type parameter T, where T is a subclass of Page and has a parameterless constructor
    // This is specified by the where T : Page, new() constraint
    public async Task NavigateToPageAsync<T>() where T : Page, new()
    {
        // Create a new instance of the page of type T
        var page = new T();

        // Use the INavigation object to push the new page onto the navigation stack, navigating to it
        await _navigation.PushAsync(page);
    }
}

