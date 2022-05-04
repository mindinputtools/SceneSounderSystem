using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
#if Windows
using Microsoft.Win32;
#endif
using MITAudioLib;
namespace ESpeakSynthSharp
{
    public class ESpeakSynth
    {
        private string dataPath = "";
        private Client client;

        public ESpeakSynth()
        {
            if (!MITAudio.IsAudioOpen())
                throw new Exception("ESpeakSynthSharp: Audio device is not open, please use MITAudio.OpenAduio() first!");
#if Windows
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\eSpeak NG"))
            {
                if (key != null)
                {
                    dataPath = key.GetValue("Path") as string;
                    SetDllDirectory(dataPath);
                }
                else
                {
                    IsInstalled = false;
                    Console.WriteLine("ESpeak isn't installed!");
                }
            }
#endif
            if (IsInstalled)
            {
                client = new Client();
                client.Initialize(dataPath);
            }
        }

        public bool IsInstalled { get; } = true;
        public bool IsSpeaking
        {
            get
            {
                if (client._isProcessing) return true;
                else if ( client.eventHandler._player.IsPlaying) return true;
                else return false;
            }
        }
        public bool Speak(string text)
        {
            if (!IsInstalled) return false;

            var r = client.Speak(text);
            return r;
        }
        public void Stop()
        {
            client._isProcessing = false;
            client.eventHandler._player.Stop();
            client.Stop();
        }
        public void Terminate()
        {
            client.Terminate();
        }
#if Windows
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SetDllDirectory(string lpPathName);
#endif
    }
}
