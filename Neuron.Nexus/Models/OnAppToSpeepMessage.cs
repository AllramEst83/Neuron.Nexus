using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuron.Nexus.Models
{
    public class OnAppToSpeepMessage
    {
        public DateTime TimeStamp { get; }

        public OnAppToSpeepMessage()
        {
            TimeStamp = DateTime.Now;
        }
    }
}
