using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech;
using Microsoft.Extensions.Options;
using Neuron.Nexus.Models;

namespace Neuron.Nexus.Services
{
    public interface ISpeechSynthesizerService
    {
        Task<byte[]> SynthesisText(string text, string targetLangugae);
    }
    public class SpeechSynthesizerService : ISpeechSynthesizerService
    {
        private readonly AppSettings appSesttings;
        public SpeechSynthesizerService(IOptions<AppSettings> appSettings)
        {
            appSesttings = appSettings.Value;
        }

        public async Task<byte[]> SynthesisText(string text, string targetLangugae)
        {

            SpeechSynthesisResult result;
            using (var stream = AudioOutputStream.CreatePullStream())
            {
                var speechConfig = SpeechConfig.FromSubscription(appSesttings.AzureSubscriptionKey, appSesttings.AzureRegion);
                speechConfig.SpeechSynthesisLanguage = targetLangugae;
                //speechConfig.SpeechSynthesisVoiceName = targetLanguageVoice;
                //"en-US-JennyNeural"

                using var audioConfig = AudioConfig.FromStreamOutput(stream);
                using var synthesizer = new SpeechSynthesizer(speechConfig, audioConfig);

                result = await synthesizer.SpeakTextAsync(text);
            }

            return result.AudioData;
        }        
    }
}
