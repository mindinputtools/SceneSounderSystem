using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Audio.OpenAL;
namespace MITAudioLib
{
    public class MITAudioPlayer : IDisposable
    {
        public delegate short[] GetBufferDelegate();
        public Action PlaybackEnded = null;
        public MITAudioPlayer(int sampleRate, ALFormat format, GetBufferDelegate fillBuffer = null)
        {
            SampleRate = sampleRate;
            Format = format;
            FillBuffer = fillBuffer;
            source = AL.GenSource();
            numBuffers = FillBuffer != null ? 2 : 1;
            buffers = AL.GenBuffers(numBuffers);
        }
        public void SetBuffer(short[] buf)
        {

            
            AL.BufferData(buffers[0], Format, ref buf[0], buf.Length * 2, SampleRate);
            AL.Source(source, ALSourcei.Buffer, buffers[0]);
        }

        public void Play()
        {
            shouldPlay = true;
            starting = true;
            if (source == 0)
            { // Sound device were prob not open during init
                source = AL.GenSource();
                buffers = AL.GenBuffers(numBuffers);
            }
            if (FillBuffer != null)
            {
                for (int i = 0; i < numBuffers; i++)
                {
                    var buffer = FillBuffer();
                    AL.BufferData(buffers[i], Format, ref buffer[0], buffer.Length * 2, SampleRate);
                    AL.SourceQueueBuffer(source, buffers[i]);

                }
            }
            else if (buffers[0] == 0)
                throw new Exception("No buffer set!");
            Task.Run(() => playerLoop());
        }

        private void playerLoop()
        {

            AL.SourcePlay(source);
            starting = false;
            while (shouldPlay && IsPlaying)
            {
                if (FillBuffer != null)
                {
                    AL.GetSource(source, ALGetSourcei.BuffersProcessed, out int procBufs);
                    if (procBufs >0)
                    {
                        var freeBuf = AL.SourceUnqueueBuffer(source);
                        var buffer = FillBuffer();
                        AL.BufferData(freeBuf, Format, ref buffer[0], buffer.Length * 2, SampleRate);
                        AL.SourceQueueBuffer(source, freeBuf);
                    }
                }
                Task.Delay(10);
            }
            AL.SourceStop(source);
            AL.SourceRewind(source);
            shouldPlay = false;
            AL.GetSource(source, ALGetSourcei.BuffersQueued, out int queuedBufs);
            if (queuedBufs > 0)
            {
                for (int i = 0; i < queuedBufs; i++)
                {
                    AL.SourceUnqueueBuffer(source);
                }
                

            }

            stopping = false;
            if (PlaybackEnded != null) PlaybackEnded();
        }

        public void Stop()
        {
            shouldPlay = false;
            stopping = true;
        }

        public int SampleRate { get; }
        public ALFormat Format { get; }
        public GetBufferDelegate FillBuffer { get; }
        private protected bool shouldPlay { get; set; }
        public bool IsPlaying
        {
            get
            {
                if (source == 0) return false;
                if (starting || stopping) return true;
                return AL.GetSourceState(source) == ALSourceState.Playing;
            }
        }
        private protected int source;
        private int numBuffers;

        private int[] buffers;
        private bool starting;
        private bool stopping;

        public void Dispose()
        {
            AL.DeleteBuffers(numBuffers, buffers);
            AL.DeleteSource(source);
        }
    }
}
