#if ANDROID
using Android.Media;

namespace Neuron.Nexus.Services;
public interface IAndroidAudioPlayerService
{
    Task PlayAudio(string translatedMessage, string fullLanguageCode);
}

public class AndroidAudioPlayerService : IAndroidAudioPlayerService
{
    private readonly ISpeechSynthesizerService speechSynthesizerService;
    private readonly MediaPlayer mediaPlayer;

    public AndroidAudioPlayerService(
        ISpeechSynthesizerService speechSynthesizerService)
    {
        this.speechSynthesizerService = speechSynthesizerService;
        mediaPlayer = new MediaPlayer();
    }

    public async Task PlayAudio(string translatedMessage, string fullLanguageCode)
    {
        if (mediaPlayer != null && !string.IsNullOrEmpty(translatedMessage) && !string.IsNullOrEmpty(fullLanguageCode))
        {
            var audioBytes = await speechSynthesizerService.SynthesisText(translatedMessage, fullLanguageCode);

            using MemoryStream stream = new(audioBytes);
            var tempFilePath = Path.Combine(Android.App.Application.Context.CacheDir.AbsolutePath, "tempAudioFile.mp3");
            using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
            {
                stream.WriteTo(fileStream);
            }

            mediaPlayer.Reset();
            mediaPlayer.SetDataSource(tempFilePath);
            mediaPlayer.Prepare();
            mediaPlayer.Start();

            mediaPlayer.Completion += (sender, e) =>
            {
                File.Delete(tempFilePath);
            };
        }
    }
}
#endif
