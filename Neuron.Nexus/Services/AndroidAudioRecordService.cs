# if ANDROID
using Android.Media;

namespace Neuron.Nexus.Services
{
    public interface IAndroidAudioRecordService
    {
        void StartRecord();
        void StopRecord();
        void ResetRecord();
        Task<int> GetAudioStream();
    }
    public class AndroidAudioRecordService : IAndroidAudioRecordService
    {
        private AudioRecord audioRecord;
        private bool isRecordStarted = false;
        private int sampleRate = 44100;
        private ChannelIn channelConfig = ChannelIn.Mono;
        private Encoding audioFormat = Encoding.Pcm16bit;
        private int bufferSize = 1024;
        private byte[] audioBuffer;

        public AndroidAudioRecordService()
        {
                audioBuffer = new byte[bufferSize];
        }
        public void StartRecord()
        {
            if (audioRecord == null)
            {
                audioRecord = new AudioRecord(AudioSource.Mic, sampleRate, channelConfig, audioFormat, bufferSize);
                audioRecord.Release();
                audioRecord.StartRecording();
            }
            else
            {
                audioRecord.StartRecording();
            }

            isRecordStarted = true;
        }

        public void ResetRecord()
        {
            if (audioRecord != null)
            {
                audioRecord.Release();
            }
            audioRecord = null;
            isRecordStarted = false;
        }
        public void StopRecord()
        {
            if (audioRecord == null)
            {
                return;
            }

            audioRecord.Stop();
            audioRecord = null;
            isRecordStarted = false;
        }
        public async Task<int> GetAudioStream()
        {
            return await audioRecord.ReadAsync(audioBuffer, 0, audioBuffer.Length);
        }
    }
}
#endif
