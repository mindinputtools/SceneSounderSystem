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
    public static class ESpeakSynth
    {
        private static string dataPath = "";

        static ESpeakSynth()
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
                Client.Initialize(dataPath);
        }

        public static bool IsInstalled { get; } = true;
        public static bool IsSpeaking
        {
            get
            {
                if (Client._isProcessing) return true;
                else if (EventHandler._player.IsPlaying) return true;
                else return false;
            }
        }
        public static bool Speak(string text)
        {
            if (!IsInstalled) return false;

            var r = Client.Speak(text);
            return r;
        }
#if Windows
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SetDllDirectory(string lpPathName);
#endif
    }
}
