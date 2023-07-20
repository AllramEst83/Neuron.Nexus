namespace Neuron.Nexus.Models
{
    public class AppDisappearingMessage
    {
        public DateTime TimeStamp { get; }
        public string Reason { get; }

        public AppDisappearingMessage(string reason)
        {
            TimeStamp = DateTime.Now;
            Reason = reason;
        }
    }
}
