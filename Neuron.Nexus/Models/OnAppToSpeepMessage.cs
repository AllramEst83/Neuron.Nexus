using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuron.Nexus.Models
{
    public class OnAppToSleepMessage
    {
        public DateTime TimeStamp { get; }

        public OnAppToSleepMessage()
        {
            TimeStamp = DateTime.Now;
        }
    }
}
