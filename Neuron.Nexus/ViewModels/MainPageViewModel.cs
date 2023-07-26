using Neuron.Nexus.Pages;
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
    public ICommand NavigateToSelectLanguagekCommand { get; private set; }
    public ICommand NavigateToSpeechCommand { get; private set; }

    // Define a constructor that takes an INavigationService parameter. 
    // This is Dependency Injection (DI) in action - it allows for loose coupling and easy unit testing.
    public MainPageViewModel()
    {
        NavigateToSummarizeDocumentsCommand = new Command(async () => await Shell.Current.GoToAsync($"{nameof(SummarizeDocumentsPage)}"));
        NavigateToSelectLanguagekCommand = new Command(async () => await Shell.Current.GoToAsync($"{nameof(SelectLanguagePage)}"));
        NavigateToSpeechCommand = new Command(async () => await Shell.Current.GoToAsync($"{nameof(SpeechPage)}"));
    }
}

