using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using craftersmine.EtherEngine.Content;
using craftersmine.EtherEngine.Utils;
using NAudio.Wave;

namespace craftersmine.EtherEngine.Core
{
    public sealed class AudioChannel
    {
        private float chlVolume;

        private WaveOut waveOut { get; set; }
        private LoopStream loopStream { get; set; }

        public Audio Audio { get; set; }
        public string ChannelName { get; internal set; }
        /// <summary>
        /// Gets or sets channel sound volume
        /// </summary>
        public float ChannelVolume { get { return chlVolume; } set { chlVolume = value; waveOut.Volume = chlVolume; } }
        /// <summary>
        /// Gets true if <see cref="AudioChannel"/> is playing sound, else false
        /// </summary>
        public bool IsPlaying { get; internal set; }
        /// <summary>
        /// Gets true if <see cref="AudioChannel"/> is paused sound, else false
        /// </summary>
        public bool IsPaused { get; internal set; }
        /// <summary>
        /// Gets true if <see cref="AudioChannel"/> is looping sound, else false
        /// </summary>
        public bool IsRepeating { get { return loopStream.EnableLooping; } set { loopStream.EnableLooping = value; } }

        public AudioChannel(string name, Audio audio)
        {
            Audio = audio;
            ChannelName = name;
            waveOut = new WaveOut();
            loopStream = new LoopStream(Audio.GetWaveFile());
            waveOut.Init(loopStream);
        }

        /// <summary>
        /// Plays assigned <see cref="Audio"/> from start
        /// </summary>
        public void Play()
        {
            waveOut.Play();
            IsPlaying = true;
        }

        /// <summary>
        /// Stops assigned <see cref="Audio"/> and sets position to 0
        /// </summary>
        public void Stop()
        {
            waveOut.Stop();
            IsPlaying = false;
        }

        /// <summary>
        /// Pauses assigned <see cref="Audio"/>
        /// </summary>
        public void Pause()
        {
            waveOut.Pause();
            IsPaused = true;
        }

        /// <summary>
        /// Resumes assigned paused <see cref="Audio"/> from pause position
        /// </summary>
        public void Resume()
        {
            waveOut.Resume();
            IsPaused = false;
        }

        /// <summary>
        /// Sets output device
        /// </summary>
        /// <param name="deviceNumber">Output device number</param>
        public void SetOutputDevice(int deviceNumber)
        {
            waveOut.DeviceNumber = deviceNumber;
        }
    }
}
