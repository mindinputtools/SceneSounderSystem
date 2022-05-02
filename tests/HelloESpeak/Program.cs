using System;
using ESpeakSynthSharp;
using MITAudioLib;
namespace HelloESpeak
{
    class Program
    {
        static void Main(string[] args)
        {
            MITAudio.OpenAudio();
            ESpeakSynth.Speak("Hello world of speech!");
            while (ESpeakSynth.IsSpeaking) ;
            ESpeakSynth.Speak("Let's get going!");
            while (ESpeakSynth.IsSpeaking) ;

            MITAudio.CloseAudio();
        }
    }
}
