using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.EtherEngine.Core
{
    public sealed class Coroutine
    {
        public CoroutineCallback Callback { get; private set; }

        public Coroutine(CoroutineCallback callback)
        {
            Callback = callback;
        }

        internal void OnUpdateCall()
        {
            GameApplication.GameWnd.BeginInvoke(Callback);
        }
    }

    public delegate void CoroutineCallback();
}
