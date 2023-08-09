namespace Neuron.Nexus.Models
{
    public class AzureKeys
    {
        public string AzureSubscriptionKey { get; set; }
        public string AzureRegion { get; set; }
    }
    public class AppSettings
    {
        public AzureKeys AzureKeys { get; set; }
        public string DeveloperEmail { get; set; }
        public string AppCenterSecret { get; set; }
    }
}
