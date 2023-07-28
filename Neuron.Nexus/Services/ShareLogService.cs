
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Extensions.Options;
using Neuron.Nexus.Models;
using Font = Microsoft.Maui.Font;

namespace Neuron.Nexus.Services
{
    public interface IShareLogService
    {
        Task ShareLogFileAsync();
    }

    public class ShareLogService : IShareLogService
    {
        private readonly AppSettings appSettings;

        public ShareLogService(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }

        public async Task ShareLogFileAsync()
        {

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var filePath = Path.Combine(documents, "crashlog.txt");

            if (File.Exists(filePath))
            {
                await Application.Current.MainPage.DisplayAlert("Log", "A crash log file could not be found.", "Ok");
                return;
            }

            if (Email.Default.IsComposeSupported)
            {
                var emailMessage = new EmailMessage
                {
                    Subject = "Crash Log",
                    Body = "Attached is the crash log for review.",
                    To = new List<string> { appSettings.DeveloperEmail },
                    Attachments = new List<EmailAttachment>
                    {
                        new EmailAttachment(filePath)
                    }
                };

                await Email.ComposeAsync(emailMessage);
            }
            else
            {
                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "Share Crash Log",
                    File = new ShareFile(filePath)
                });
            }
        }
    }
}
