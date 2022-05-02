﻿using System;
using System.Runtime.InteropServices;

namespace ESpeakSynthSharp
{
    internal class Client
    {
        enum AudioOutput
        {
            Playback,
            Retrieval,
            Synchronous,
            SynchronousPlayback
        };

        enum Error
        {
            EE_OK = 0,
            EE_INTERNAL_ERROR = -1,
            EE_BUFFER_FULL = 1,
            EE_NOT_FOUND = 2
        }

        enum PositionType
        {
            Character = 1,
            Word = 2,
            Sentence = 3
        }

        enum Parameter
        {
            Rate = 1,
            Volume = 2,
            Pitch = 3,
            Range = 4,
            Punctuation = 5,
            Capitals = 6,
            WordGap = 7,
            Intonation = 9,
        }

        enum ParameterType
        {
            Absolute = 0,
            Relative = 1
        }

        [Flags]
        enum SpeechFlags
        {
            CharsUtf8 = 1,
            SSML = 0x10,
        }

        static bool Initialized = false;
        internal static bool _isProcessing { get; set; } = false;
        internal static EventHandler.SynthCallback cb = EventHandler.Handle; // To prevent garbage collection of the callback that is used in unsafe space, we store it here
        public static void Initialize(string path)
        {
            var result = espeak_Initialize(AudioOutput.Retrieval, 0, path, 0);
            if (result == (int)Error.EE_INTERNAL_ERROR)
            {
                throw new Exception(string.Format("Could not initialize ESpeak. Maybe there is no espeak data at {0}?", path));
            }

            espeak_SetSynthCallback(cb);

            Initialized = true;
        }

        public static bool SetRate(int rate)
        {
            if (rate < 80 && rate > 450)
            {
                throw new Exception("The rate must be between 80 and 450.");
            }

            var result = espeak_SetParameter(Parameter.Rate, rate, ParameterType.Absolute);
            return CheckResult(result);
        }

        static bool CheckResult(Error result)
        {
            if (result == Error.EE_OK)
            {
                return true;
            }
            else if (result == Error.EE_BUFFER_FULL)
            {
                _isProcessing = false;
                return false;
            }
            else if (result == Error.EE_INTERNAL_ERROR)
            {
                throw new Exception("Internal error in ESpeak.");
            }
            else
            {
                _isProcessing = false;
                return false;
            }
        }

        public static bool Speak(string text)
        {
            _isProcessing = true;
            var result = espeak_Synth(text, text.Length * Marshal.SystemDefaultCharSize);
            
            return CheckResult(result);
        }

        public static bool SpeakSSML(string text)
        {
            var result = espeak_Synth(text, text.Length * Marshal.SystemDefaultCharSize, 0, PositionType.Character, 0, SpeechFlags.CharsUtf8 | SpeechFlags.SSML);
            return CheckResult(result);
        }

        public static bool Stop()
        {
            var result = espeak_Cancel();
            return CheckResult(result);
        }

        public static bool SetVoiceByName(string name)
        {
            var result = espeak_SetVoiceByName(name);
            return CheckResult(result);
        }

        public static Voice GetCurrentVoice()
        {
            IntPtr espeakVoicePtr = espeak_GetCurrentVoice();
            ESpeakVoice espeakVoice = (ESpeakVoice)Marshal.PtrToStructure(espeakVoicePtr, typeof(ESpeakVoice));

            if (espeakVoice.Equals(default(ESpeakVoice)))
            {
                throw new Exception("eSpeak returned an empty voice object. Did you call one of the ESpeak.SetVoice*() functions?");
            }

            return new Voice()
            {
                Name = espeakVoice.Name,
                Languages = espeakVoice.Languages.Substring(1),
                Priority = (int)espeakVoice.Languages[0],
                Identifier = espeakVoice.Identifier,
            };
        }

        [DllImport("libespeak-ng", CharSet = CharSet.Auto)]
        static extern Error espeak_SetVoiceByName([MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport("libespeak-ng", CharSet = CharSet.Auto)]
        static extern Error espeak_SetParameter(Parameter parameter, int value, ParameterType type);

        [DllImport("libespeak-ng", CharSet = CharSet.Auto)]
        static extern IntPtr espeak_GetCurrentVoice();

        [DllImport("libespeak-ng", CharSet = CharSet.Auto)]
        static extern Error espeak_Synth([MarshalAs(UnmanagedType.LPUTF8Str)] string text, int size, uint startPosition = 0, PositionType positionType = PositionType.Character, uint endPosition = 0, SpeechFlags flags = SpeechFlags.CharsUtf8, UIntPtr uniqueIdentifier = default(UIntPtr), IntPtr userData = default(IntPtr));

        [DllImport("libespeak-ng", CharSet = CharSet.Auto)]
        static extern int espeak_Initialize(AudioOutput output, int bufferLength, string path, int options);

        [DllImport("libespeak-ng", CharSet = CharSet.Auto)]
        static extern Error espeak_Cancel();

        [DllImport("libespeak-ng", CharSet = CharSet.Auto)]
        static extern void espeak_SetSynthCallback(EventHandler.SynthCallback callback);
    }
}
