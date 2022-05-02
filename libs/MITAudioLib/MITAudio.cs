using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Audio.OpenAL;

namespace MITAudioLib
{
    public static class MITAudio
    {
        internal static ALDevice CurrentDevice = ALDevice.Null;

        internal static ALContext CurrentContext = ALContext.Null;
        private static bool audioIsOpen;

        public static bool OpenAudio()
        {
            if (audioIsOpen) return true; ;
            CurrentDevice = ALC.OpenDevice(null);
            CurrentContext = ALC.CreateContext(CurrentDevice, (int[])null);
            ALC.MakeContextCurrent(CurrentContext);
            ALError error = AL.GetError();
            if (error == ALError.NoError)
            {
                audioIsOpen = true;
                return true;
            }
            else return false;
        }
        public static void CloseAudio()
        {
            if (audioIsOpen)
            {
                ALC.MakeContextCurrent(ALContext.Null);
                ALC.DestroyContext(CurrentContext);
                ALC.CloseDevice(CurrentDevice);
                audioIsOpen = false;
            }
        }
        public static void CheckALError(string str)
        {
            ALError error = AL.GetError();
            if (error != ALError.NoError)
            {
                Console.WriteLine($"ALError at '{str}': {AL.GetErrorString(error)}");
            }
        }

        public static void FillSine(short[] buffer, float frequency, float sampleRate)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (short)(MathF.Sin((i * frequency * MathF.PI * 2) / sampleRate) * short.MaxValue);
            }
        }

        public static void PrintDevices()
        {
            var devices = ALC.GetStringList(GetEnumerationStringList.DeviceSpecifier);
            Console.WriteLine($"Devices: {string.Join(", ", devices)}");
            string deviceName = ALC.GetString(ALDevice.Null, AlcGetString.DefaultDeviceSpecifier);
            Console.WriteLine($"Default device: {deviceName}");
        }
        public static bool IsAudioOpen()
        {
            return audioIsOpen;
        }
    }
}
