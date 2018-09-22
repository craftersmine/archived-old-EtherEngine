using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace craftersmine.EtherEngine.Core
{
    public sealed class Coroutine
    {
        private Timer CoroutineThread { get; set; }
        private Timer CoroutineTPSCalc { get; set; }
        private CoroutineMethod Method { get; set; }
        private int LastTPS { get; set; }
        
        public int TPS { get; internal set; }
        public int Ticks { get; internal set; }
        public string Name { get; internal set; }
        public bool IsRunning { get; internal set; }

        public Coroutine(CoroutineMethod coroutineMethod, string coroutineName)
        {
            Method = coroutineMethod;
            Name = coroutineName;
        }

        public void StartCoroutine()
        {
            IsRunning = true;
            CoroutineTPSCalc = new Timer(new TimerCallback(TPSCalc), null, 0, 1000);
            CoroutineThread = new Timer(new TimerCallback(CoroutineMethodExecutor), null, 20, 10);
            GameApplication.Log(Utils.LogEntryType.Info, "Started coroutine: " + Name);
        }

        public void StopCoroutine()
        {
            IsRunning = false;
            CoroutineThread.Change(Timeout.Infinite, 10);
            CoroutineTPSCalc.Change(Timeout.Infinite, 1000);
            GameApplication.Log(Utils.LogEntryType.Info, "Stopped coroutine: " + Name);
        }

        private void TPSCalc(object state)
        {
            TPS = Ticks - LastTPS;
            LastTPS = Ticks;
        }

        private void CoroutineMethodExecutor(object state)
        {
            Method?.Invoke();
            Ticks++;
        }
    }

    public delegate void CoroutineMethod();
}
