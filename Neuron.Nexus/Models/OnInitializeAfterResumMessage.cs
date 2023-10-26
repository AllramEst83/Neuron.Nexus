namespace Neuron.Nexus.Models
{
    public class OnInitializeAfterResumMessage
    {
        public DateTime TimeStamp { get; }

        public OnInitializeAfterResumMessage()
        {
            TimeStamp = DateTime.Now;
        }
    }
}
