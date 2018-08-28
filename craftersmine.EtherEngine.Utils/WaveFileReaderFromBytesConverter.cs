using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using NAudio.Wave;

namespace craftersmine.EtherEngine.Utils
{
    /// <summary>
    /// Gives static method of conversion <code>byte</code> array into <see cref="WaveFileReader"/>
    /// </summary>
    public sealed class WaveFileReaderFromBytesConverter
    {
        /// <summary>
        /// Converts <code>byte</code> array into <see cref="WaveFileReader"/>
        /// </summary>
        /// <param name="byteArrayIn">Input of <code>byte</code> array</param>
        /// <returns>Returns <see cref="WaveFileReader"/></returns>
        public static WaveFileReader ByteArrayToWaveFileReader(byte[] byteArrayIn)
        {
            WaveFileReader returnWaveReader = null;
            if (byteArrayIn != null)
            {
                MemoryStream ms = new MemoryStream(byteArrayIn);
                ms.Position = 0;
                returnWaveReader = new WaveFileReader(ms);
            }
            return returnWaveReader;
        }
    }
}
