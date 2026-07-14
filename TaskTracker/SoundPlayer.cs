using System; 
using NAudio;
using NAudio.Wave;

namespace TaskTracker
{
    public class SoundPlayer
    {
        public static void PlayBell()
        {
            string path = "bellSound.mp3";

            using var audioFile = new AudioFileReader(path);
            using var outputDevice = new WaveOutEvent();

            outputDevice.Init(audioFile);
            outputDevice.Play();

            while (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                //Thread.Sleep(100);
            }
        }
    }
}