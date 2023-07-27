namespace Neuron.Nexus.Services
{
    public class LogService
    {
        public async Task ShareLogFileAsync()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var filePath = Path.Combine(documents, "crashlog.txt");

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Share Crash Log",
                File = new ShareFile(filePath)
            });
        }
    }
}
