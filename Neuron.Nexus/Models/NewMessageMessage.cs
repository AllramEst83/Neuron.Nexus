namespace Neuron.Nexus.Models
{
    public class NewMessageMessage
    {
        public DateTime TimeStamp { get; }
        public string Message { get; }

        public NewMessageMessage(string message)
        {
            TimeStamp = DateTime.Now;
            Message = message;
        }
    }


}
