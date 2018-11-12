using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace craftersmine.EtherEngine.Core
{
    public sealed class GameUpdater
    {
        private int TPSLastTick { get; set; }

        internal AccurateTimer GameUpdaterTimer { get; private set; }
        internal DispatcherTimer DebugInfoUpdater { get; private set; }

        internal int CurrentTick { get; private set; }
        internal int TPSCounter { get; private set; }

        public GameUpdater()
        {
            GameUpdaterTimer = new AccurateTimer(new Action(Update), 40);
            DebugInfoUpdater = new DispatcherTimer();
            DebugInfoUpdater.Interval = TimeSpan.FromSeconds(1d);
            DebugInfoUpdater.Tick += UpdateDebugInfo;
        }

        private void UpdateDebugInfo(object sender, EventArgs e)
        {
            TPSCounter = CurrentTick - TPSLastTick;
            TPSLastTick = CurrentTick;
        }

        public void Update()
        {
            if (GameApplication.GameWnd.CurrentScene != null)
            {
                if (GameApplication.GameWnd.CurrentScene.Coroutines.Count > 0)
                {
                    for (int coroutineIndex = 0; coroutineIndex < GameApplication.GameWnd.CurrentScene.Coroutines.Count; coroutineIndex++)
                        GameApplication.GameWnd.CurrentScene.Coroutines[coroutineIndex].OnUpdateCall();
                }
                GameApplication.GameWnd.CurrentScene.OnSceneUpdateInternal();
                if (GameApplication.GameWnd.CurrentScene.GameObjects.Count > 0)
                {
                    for (int gameObjectIndex = 0; gameObjectIndex < GameApplication.GameWnd.CurrentScene.GameObjects.Count; gameObjectIndex++)
                        GameApplication.GameWnd.CurrentScene.GameObjects[gameObjectIndex].OnUpdateInternal();
                }
            }
            if (CurrentTick == int.MaxValue) CurrentTick = 0;
            CurrentTick++;
        }

        public void StartUpdater()
        {
            GameUpdaterTimer.Start();
            DebugInfoUpdater.Start();
        }

        public void StopUpdater()
        {
            DebugInfoUpdater.Stop();
            GameUpdaterTimer.Stop();
        }
    }
}
