using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;

namespace craftersmine.EtherEngine.Content
{
    /// <summary>
    /// Represents wave audio file
    /// </summary>
    public class Audio
    {
        private WaveFileReader waveFile { get; set; }
        /// <summary>
        /// <see cref="Audio"/> total time
        /// </summary>
        public TimeSpan FileLength { get; internal set; }

        /// <summary>
        /// Creates new <see cref="Audio"/> instance from <see cref="WaveFileReader"/>
        /// </summary>
        /// <param name="waveFile"><see cref="WaveFileReader"/> with file</param>
        public Audio(WaveFileReader waveFile)
        {
            this.waveFile = waveFile;
            FileLength = this.waveFile.TotalTime;
        }

        /// <summary>
        /// Gets <see cref="WaveFileReader"/> from <see cref="Audio"/>
        /// </summary>
        /// <returns><see cref="WaveFileReader"/></returns>
        public WaveFileReader GetWaveFile()
        {
            return waveFile;
        }

        /// <summary>
        /// Loads wave audio from file
        /// </summary>
        /// <param name="filepath">Filepath to wave audio</param>
        /// <returns></returns>
        public static Audio FromFile(string filepath)
        {
            try
            {
                WaveFileReader waveFileReader = new WaveFileReader(filepath);
                Audio audio = new Audio(waveFileReader);
                return audio;
            }
            catch (Exception ex)
            {
                throw new ContentLoadException("Unable to load audio from " + filepath + "! Inner exception message: " + ex.Message, ex);
            }
        }
    }
}
