#if IOS
using AVFoundation;
using Foundation;
using System.IO;

namespace Neuron.Nexus.Services
{
    public interface IAudioRecorderService
    {
        void StartRecord();
        string StopRecord();
        void PauseRecord();
        void ResetRecord();
    }

    public class IOSAudioRecorderService : IAudioRecorderService
    {
        //https://www.syncfusion.com/blogs/post/building-an-audio-recorder-and-player-app-in-net-maui.aspx

        readonly AVAudioRecorder recorder;
        readonly NSUrl url;
        readonly NSError error;
        readonly NSDictionary settings;
        readonly string audioFilePath;

        public IOSAudioRecorderService()
        {
            var audioSession = AVAudioSession.SharedInstance();
            var err = audioSession.SetCategory(AVAudioSessionCategory.PlayAndRecord);
            if (err != null)
            {
                Console.WriteLine("audioSession: {0}", err);
                return;
            }
            err = audioSession.SetActive(true);
            if (err != null)
            {
                Console.WriteLine("audioSession: {0}", err);
                return;
            }

            string fileName = "/Record_" + DateTime.UtcNow.ToString("ddMMM_hhmmss") + ".wav";
            var docuFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            audioFilePath = docuFolder + fileName;
            url = NSUrl.FromFilename(audioFilePath);

            NSObject[] values = new NSObject[]
            {
                      NSNumber.FromFloat(44100.0f),
                      NSNumber.FromInt32((int)AudioToolbox.AudioFormatType.LinearPCM),
                      NSNumber.FromInt32(1),
                      NSNumber.FromInt32(16),
                      NSNumber.FromBoolean(false),
                      NSNumber.FromBoolean(false)
            };
            NSObject[] key = new NSObject[]
            {
                      AVAudioSettings.AVSampleRateKey,
                      AVAudioSettings.AVFormatIDKey,
                      AVAudioSettings.AVNumberOfChannelsKey,
                      AVAudioSettings.AVLinearPCMBitDepthKey,
                      AVAudioSettings.AVLinearPCMIsBigEndianKey,
                      AVAudioSettings.AVLinearPCMIsFloatKey
            };
            settings = NSDictionary.FromObjectsAndKeys(values, key);

            recorder = AVAudioRecorder.Create(url, new AudioSettings(settings), out error);
            recorder.PrepareToRecord();
        }

        public void PauseRecord() => recorder.Pause();

        public void ResetRecord()
        {
            if (recorder != null)
            {
                recorder.Dispose();
                File.Delete(audioFilePath);
            }
        }

        public void StartRecord() => recorder.Record();

        public string StopRecord()
        {
            if (recorder == null)
            {
                return string.Empty;
            }
            recorder.Stop();
            return audioFilePath;
        }
    }
}
#endif
