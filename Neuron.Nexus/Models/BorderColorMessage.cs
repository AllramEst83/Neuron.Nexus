using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuron.Nexus.Models
{
    public class BorderColorMessage
    {
        public DateTime TimeStamp { get; }
        public ButtonsEnum Button { get; }

        public BorderColorMessage(ButtonsEnum button)
        {
            TimeStamp = DateTime.Now;
            Button = button;
        }
    }
}
