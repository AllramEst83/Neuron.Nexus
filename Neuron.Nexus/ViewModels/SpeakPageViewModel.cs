using CommunityToolkit.Mvvm.Messaging;
using Neuron.Nexus.Models;
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
        //Commands
        public ICommand SpeakLanguageOneCommand { get; private set; }
        public ICommand SpeakLanguageTwoCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
        public ICommand StartCommand { get; private set; }
        public ICommand SelectedIndexChangedCommandOne { get; private set; }
        public ICommand SelectedIndexChangedCommandTwo { get; private set; }

        //Collections
        public ObservableCollection<string> Languages { get; set; } = new();

        //UI Booleans
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
            //Commands
            SpeakLanguageOneCommand = new Command(() =>
            {
                WeakReferenceMessenger.Default.Send(new AnimateButtonMessage(AnimationButtonsEnum.LanguageOneBtn));
            });
            SpeakLanguageTwoCommand = new Command(() =>
            {

                WeakReferenceMessenger.Default.Send(new AnimateButtonMessage(AnimationButtonsEnum.LanguageTwoBtn));
            });
            StopCommand = new Command(() =>
            {
                WeakReferenceMessenger.Default.Send(new AnimateButtonMessage(AnimationButtonsEnum.StopBtn));
            });

            //Language is choosen commands
            StartCommand = new Command(() =>
            {
                IsLanguageSelectionVisible = !IsLanguageSelectionVisible;
                IsSpeakButtonsEnabled = !IsSpeakButtonsEnabled;
            });


            SelectedIndexChangedCommandOne = new Command<Picker>(HandlePickerSelectionChangedOne);
            SelectedIndexChangedCommandTwo = new Command<Picker>(HandlePickerSelectionChangedTwo);

            Languages = new ObservableCollection<string>
            {
                "English",
                "Spanish",
                "French"
            };
        }

        private void HandlePickerSelectionChangedOne(Picker picker)
        {
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex != -1)
            {
                string selectedLanguage = (string)picker.ItemsSource[selectedIndex];
                // Do something with the selected color...
            }  // handle the selection change
        }

        private void HandlePickerSelectionChangedTwo(Picker picker)
        {
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex != -1)
            {
                string selectedLanguage = (string)picker.ItemsSource[selectedIndex];
                // Do something with the selected color...
            }  // handle the selection change
        }
    }
}
