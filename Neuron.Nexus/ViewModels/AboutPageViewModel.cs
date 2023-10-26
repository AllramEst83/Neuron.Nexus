using CommunityToolkit.Mvvm.Input;

namespace Neuron.Nexus.ViewModels
{
    public partial class AboutPageViewModel : BaseViewModel
    {


        [RelayCommand]
        async Task GoToHyperlinkAdress(string adress)
        {
            await Launcher.OpenAsync(adress);
        }
    }
}
