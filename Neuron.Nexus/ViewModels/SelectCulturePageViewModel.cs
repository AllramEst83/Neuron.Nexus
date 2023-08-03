
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using Neuron.Nexus.Managers;
using Neuron.Nexus.Resources.Languages;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Neuron.Nexus.ViewModels
{
    public partial class SelectCulturePageViewModel : BaseViewModel
    {
        private ObservableCollection<string> _cultures;
        public ObservableCollection<string> Cultures
        {
            get => _cultures;
            set => SetProperty(ref _cultures, value);
        }
        public SelectCulturePageViewModel()
        {
            Cultures = new ObservableCollection<string>() { "sv-SE","en-US" };

        }

        [RelayCommand]
        async Task SetLanguage(string culture)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                await SetApplangugaes(culture);
            }
        }

        private async Task SetApplangugaes(string culture)
        {

            LocalizationResourceManager.Instance.SetCulture(new CultureInfo(culture));

            Preferences.Set("currentCulture", culture);

            await Toast.Make(AppResources.AppLanguagesSet,ToastDuration.Long).Show();
        }
    }
}
