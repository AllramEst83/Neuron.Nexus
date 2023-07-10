using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Neuron.Nexus.ViewModels
{
    public class SpeakPageViewModel : BaseViewModel
    {
        public ICommand SpeakLanguageOneCommand { get; private set; }
        public ICommand SpeakLanguageTwoCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
        public ICommand StartCommand { get; private set; }

        public ObservableCollection<string> Languages { get; set; } = new ();

        private bool _isSpeakButtonsEnabled = false;
        public bool IsSpeakButtonsEnabled
        {
            get { return _isSpeakButtonsEnabled; }
            set
            {
                _isSpeakButtonsEnabled = value;
                OnPropertyChanged(nameof(IsSpeakButtonsEnabled));
            }
        }

        private bool _isLanguageSelectionVisible = true;
        public bool IsLanguageSelectionVisible
        {
            get { return _isLanguageSelectionVisible; }
            set
            {
                _isLanguageSelectionVisible = value;
                OnPropertyChanged(nameof(IsLanguageSelectionVisible));
            }
        }

        public SpeakPageViewModel()
        {
            SpeakLanguageOneCommand = new Command(() => { });
            SpeakLanguageTwoCommand = new Command(() => { });
            StopCommand = new Command(() => { });
            StartCommand = new Command(() =>
            {
                IsLanguageSelectionVisible = !IsLanguageSelectionVisible;
                IsSpeakButtonsEnabled = !IsSpeakButtonsEnabled;
            });

            Languages = new ObservableCollection<string>
            {
                "English",
                "Spanish",
                "French"
            };
        }
    }
}
