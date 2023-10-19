using Microsoft.Extensions.Options;
using Neuron.Nexus.Models;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Neuron.Nexus.Services
{
    public interface IChatGptService
    {
        Task<string> GetResponseAsync(string transcription);
    }
    public class ChatGptService : IChatGptService
    {
        private readonly AppSettings appSettings;
        private readonly HttpClient _httpClient;

        public ChatGptService(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.appSettings.ChatGptSettings.ApiKey}");
        }

        public async Task<string> GetResponseAsync(string transcription)
        {
            var requestPayload = new
            {
                messages = new[]
                {
                    new { role = "system", content = "Sammanfatta det som sägs på bästa sätt, även om det är osammanhängande. Flera personer kan förekomma i texten. Om det finns listor, kan du använda formatera det med en rubrik och en punktlista." },
                    new { role = "user", content = transcription }
                },
                max_tokens = appSettings.ChatGptSettings.MaxTokens,
                temperature = appSettings.ChatGptSettings.Temperature,
                top_p = appSettings.ChatGptSettings.TopP,
                model = appSettings.ChatGptSettings.Model
            };


            var jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(requestPayload);
            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(appSettings.ChatGptSettings.EndPoint, httpContent);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                JObject parsedResponse = JObject.Parse(jsonResponse);
               return parsedResponse["choices"][0]["message"]["content"].ToString();

            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to retrieve response: {response.ReasonPhrase}. Response content: {errorContent}");
            }

        }
    }
}
