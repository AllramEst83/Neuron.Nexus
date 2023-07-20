using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuron.Nexus.Models
{
    public class OnInitializeMessage
    {
        public DateTime TimeStamp { get; }

        public OnInitializeMessage()
        {
            TimeStamp = DateTime.Now;
        }
    }
}
