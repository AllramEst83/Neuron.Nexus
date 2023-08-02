using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
