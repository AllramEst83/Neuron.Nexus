using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Neuron.Nexus.Models
{
    public class CultureOption : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string CultureDisplayName { get; set; }
        public string CultureCode { get; set; }
        public string CultureIconName { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        private bool _isPressed;
        public bool IsPressed
        {
            get => _isPressed;
            set
            {
                _isPressed = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
