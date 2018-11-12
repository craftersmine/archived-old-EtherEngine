using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.EtherEngine.Core
{
    public sealed class Coroutine
    {
        internal AccurateTimer CoroutineUpdater { get; set; }
        public CoroutineCallback Callback { get; private set; }

        public Coroutine(CoroutineCallback callback)
        {
            Callback = callback;
        }

        public void StartCoroutine()
        {
            CoroutineUpdater = new AccurateTimer(OnUpdateCall, 40);
            GameApplication.RegisteredCoroutines.Add(this);
            CoroutineUpdater.Start();
        }

        public void StopCoroutine()
        {
            CoroutineUpdater.Stop();
            GameApplication.RegisteredCoroutines.Remove(this);
        }

        internal void OnUpdateCall()
        {
             Callback?.Invoke(this);
        }
    }

    public delegate void CoroutineCallback(Coroutine callingCoroutine);
}
