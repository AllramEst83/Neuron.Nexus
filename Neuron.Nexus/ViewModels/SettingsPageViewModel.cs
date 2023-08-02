using CommunityToolkit.Mvvm.Input;
using Neuron.Nexus.Services;

namespace Neuron.Nexus.ViewModels
{
    public partial class SettingsPageViewModel : BaseViewModel
    {
        private readonly IShareLogService logService;

        public SettingsPageViewModel(IShareLogService logService)
        {
            this.logService = logService;
        }

        [RelayCommand]
        public async Task ShareLog()
        {
            await logService.ShareLogFileAsync();
        }

        [RelayCommand]
        public async Task GoToAppLanguage()
        {
            await Application.Current.MainPage.DisplayAlert("App language", "The app language page has not been created yet. Thank you for your patience.", "Ok");
        }

        [RelayCommand]
        public async Task GoToThemeSelector()
        {
            await Application.Current.MainPage.DisplayAlert("Theme", "The theme selector has not been created yet. Thank you for your patience.", "Ok");
        }
    }
}
