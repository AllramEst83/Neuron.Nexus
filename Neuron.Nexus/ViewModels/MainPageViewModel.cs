using CommunityToolkit.Mvvm.Input;
using Neuron.Nexus.Pages;
using Neuron.Nexus.Services;
using System.Windows.Input;

namespace Neuron.Nexus.ViewModels;

// This class defines the ViewModel for the Main Page. 
// It inherits from the BaseViewModel that provides basic functionalities for all ViewModel classes.
public partial class MainPageViewModel : BaseViewModel
{
    public MainPageViewModel()
    {
    }

    [RelayCommand]
    async Task NavigateToSelectLanguagePage()
    {
        await Shell.Current.GoToAsync($"{nameof(SelectLanguagePage)}");
    }
}

