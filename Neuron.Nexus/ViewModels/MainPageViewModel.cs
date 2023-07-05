using Neuron.Nexus.Services;
using System.Windows.Input;

namespace Neuron.Nexus.ViewModels;

// This class defines the ViewModel for the Main Page. 
// It inherits from the BaseViewModel that provides basic functionalities for all ViewModel classes.
public partial class MainPageViewModel : BaseViewModel
{
    // Declare ICommand properties that will be bound to the button commands in the View.
    // ICommand enables data binding command logic, thus simplifying the View (XAML).
    public ICommand NavigateToSummarizeDocumentsCommand { get; private set; }
    public ICommand NavigateToSpeakCommand { get; private set; }
    public ICommand NavigateToSpeechCommand { get; private set; }

    // Declare a readonly field for the navigation service, which will be injected via the constructor.
    private readonly INavigationService _navigationService;

    // Define a constructor that takes an INavigationService parameter. 
    // This is Dependency Injection (DI) in action - it allows for loose coupling and easy unit testing.
    public MainPageViewModel(INavigationService navigationService)
    {
        // Assign the injected INavigationService object to the private readonly field.
        _navigationService = navigationService;

        // Initialize the ICommand properties with new Commands, 
        // binding them to the respective methods in the navigation service.
        // These Commands are executed when the corresponding Command in the View (XAML) is triggered.
        NavigateToSummarizeDocumentsCommand = new Command(async () => await _navigationService.NavigateToSummarizeDocumentsPageAsync());
        NavigateToSpeakCommand = new Command(async () => await _navigationService.NavigateToSpeakPageAsync());
        NavigateToSpeechCommand = new Command(async () => await _navigationService.NavigateToSpeechPageAsync());
    }
}

