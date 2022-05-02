using System;
using System.Threading.Tasks;
using MITAudioLib;
namespace MITAudioLibTest
{
    class Program
    {
        private static int numSamples = 10000;
        private static float rate = 22050;
        static Random rnd = new();
        private static short[] genSamples()
        {
            int ns = rnd.Next(1000, 10000);
            short[] smp = new short[ns];
            
            float freq = rnd.Next(500, 5000);
            MITAudio.FillSine(smp, freq, rate);
            return smp;
        }
        static void Main(string[] args)
        {
            MITAudio.PrintDevices();
            var audioOk = MITAudio.OpenAudio();
            if (!audioOk)
            {
                Console.WriteLine("Error opening audio!");
                return;
            }
            short[] smp = new short[numSamples];
            MITAudio.FillSine(smp, 440, rate);
            using (var player = new MITAudioPlayer((int)rate, OpenTK.Audio.OpenAL.ALFormat.Mono16))
            {
                player.SetBuffer(smp);
                player.Play();
                while (player.IsPlaying)
                {
                    Task.Delay(10);
                }
            }
            using (var player = new MITAudioPlayer((int)rate, OpenTK.Audio.OpenAL.ALFormat.Mono16, fillBuffer: genSamples))
            {
                
                player.Play();
                while (player.IsPlaying)
                {
                    if (Console.KeyAvailable) player.Stop();
                    Task.Delay(10);
                }
            }

            MITAudio.CloseAudio();
        }
    }
}
