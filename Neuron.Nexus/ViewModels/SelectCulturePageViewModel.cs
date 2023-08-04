
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Neuron.Nexus.Managers;
using Neuron.Nexus.Models;
using Neuron.Nexus.Repositories;
using Neuron.Nexus.Resources.Languages;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Neuron.Nexus.ViewModels
{
    public partial class SelectCulturePageViewModel : BaseViewModel
    {
        private ObservableCollection<CultureOption> _cultures;
        private CultureOption _currentlySelectedCulture;
        private readonly ILanguageRepository languageRepository;
        private readonly string _defaultCulureCode = "en-US";

        public ObservableCollection<CultureOption> Cultures
        {
            get => _cultures;
            set => SetProperty(ref _cultures, value);
        }
        public SelectCulturePageViewModel(ILanguageRepository languageRepository)
        {
            this.languageRepository = languageRepository;
            Cultures = new ObservableCollection<CultureOption>(languageRepository.GetCultureOptions());

            var userCultureCode = Preferences.Get("currentCulture", _defaultCulureCode);

            var selectedCultureCode = Cultures.FirstOrDefault(x => x.CultureCode == userCultureCode);
            if (selectedCultureCode != null)
            {
                selectedCultureCode.IsPressed = true;
                _currentlySelectedCulture = selectedCultureCode;
            }
        }

        [RelayCommand]
        async Task SetLanguage(string culture)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                await SetApplangugaes(culture);
            }
        }

        private async Task SetApplangugaes(string cultureCode)
        {
            var tappedItem = Cultures.FirstOrDefault(c => c.CultureCode == cultureCode);
            if (tappedItem != null)
            {
                // Deselect the previously selected item
                if (_currentlySelectedCulture != null)
                {
                    _currentlySelectedCulture.IsPressed = false;
                }

                // Select the new item
                tappedItem.IsPressed = true;

                // Update the reference to the currently selected item
                _currentlySelectedCulture = tappedItem;
            }

            LocalizationResourceManager.Instance.SetCulture(new CultureInfo(cultureCode));

            Preferences.Set("currentCulture", cultureCode);

            await Toast.Make(AppResources.AppLanguagesSet,ToastDuration.Long).Show();
        }
    }
}
