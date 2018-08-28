using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.EtherEngine.Content
{
    /// <summary>
    /// Throws at content load failure
    /// </summary>
    [System.Serializable]
    public class ContentLoadException : Exception
    {
        /// <summary>
        /// Throws at content load failure
        /// </summary>
        public ContentLoadException() { }

        /// <summary>
        /// Throws at content load failure
        /// </summary>
        /// <param name="message">Error message</param>
        public ContentLoadException(string message) : base(message) { }

        /// <summary>
        /// Throws at content load failure
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="inner">Inner <see cref="Exception"/></param>
        public ContentLoadException(string message, Exception inner) : base(message, inner) { }
    }
}
