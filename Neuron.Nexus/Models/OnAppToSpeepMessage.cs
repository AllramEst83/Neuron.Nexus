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
