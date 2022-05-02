using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MITAudioLib;

namespace ESpeakSynthSharp
{
    class EventHandler
    {

        public delegate int SynthCallback(IntPtr wavePtr, int bufferLength, IntPtr eventsPtr);
        static List<short> _samples = new List<short>();
        static MemoryStream Stream;
        public static MITAudioPlayer _player = new MITAudioPlayer(22050, OpenTK.Audio.OpenAL.ALFormat.Mono16);
        public static int Handle(IntPtr wavePtr, int bufferLength, IntPtr eventsPtr)
        {
            //Console.WriteLine("Received event!");
            //Console.WriteLine("Buffer length is " + bufferLength);

            if (bufferLength == 0 && _samples.Count >0)
            {
                
                PlayAudio();
                _samples.Clear();
                Client._isProcessing = false;
                return 0;
            }

            WriteAudioToStream(wavePtr, bufferLength);

            //var events = MarshalEvents(eventsPtr);

            //foreach (Event anEvent in events)
            //{
            //    Console.WriteLine(anEvent.Type);
            //    Console.WriteLine(anEvent.Id);
            //}

            return 0; // continue synthesis
        }

        static List<Event> MarshalEvents(IntPtr eventsPtr)
        {
            var events = new List<Event>();
            int structSize = Marshal.SizeOf(typeof(Event));

            for (int i = 0; true; i++)
            {
                IntPtr data = new IntPtr(eventsPtr.ToInt64() + structSize * i);
                Event currentEvent = (Event)Marshal.PtrToStructure(data, typeof(Event));
                if (currentEvent.Type == Event.EventType.ListTerminated)
                {
                    break;
                }
                events.Add(currentEvent);
            }

            return events;
        }

        static int WriteAudioToStream(IntPtr wavePtr, int bufferLength)
        {
            if (wavePtr == IntPtr.Zero)
            {
                return 0; // Continue synthesis
            }


            short[] audio = new short[bufferLength];
            Marshal.Copy(wavePtr, audio, 0, audio.Length);
            _samples.AddRange(audio);

            return 0;
        }


        static void PlayAudio()
        {
            
            _player.SetBuffer(_samples.ToArray());
            try
            {
                _player.Play();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }


    }   
}
