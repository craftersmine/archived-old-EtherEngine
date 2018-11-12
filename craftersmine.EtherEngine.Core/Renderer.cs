using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace craftersmine.EtherEngine.Core
{
    public sealed class Renderer
    {
        private string DebugInfoData { get; set; }
        private int FPSLastTick { get; set; }

        internal AccurateTimer FrameUpdater { get; private set; }
        internal DispatcherTimer DebugInfoUpdater { get; private set; }
        internal AcceleratedCanvas AcceleratedCanvas { get; set; }
        internal Stopwatch FrameTime { get; private set; }

        internal Label DebugInfo { get; set; } = new Label() { Foreground = new SolidColorBrush(Color.FromRgb(0, 255,0)), Content = "" };

        internal int CurrentTick { get; private set; }
        internal int FPSCounter { get; private set; }

        public Renderer(AcceleratedCanvas acceleratedCanvas)
        {
            AcceleratedCanvas = acceleratedCanvas;

            FrameUpdater = new AccurateTimer(new Action(Render), 17);
            DebugInfoUpdater = new DispatcherTimer();
            DebugInfoUpdater.Interval = TimeSpan.FromSeconds(1d);
            DebugInfoUpdater.Tick += UpdateDebugInfo;

            FrameTime = new Stopwatch();

            AcceleratedCanvas.Canvas._base.Children.Add(DebugInfo);
        }

        private void Render()
        {
            DebugInfoData = string.Format("craftersmine EtherEngine - Game debug info{0}{0}  Draw Call: {1}{0}  FPS: {2}{0}  Frame Time: {3} ms{0}  TPS: {4}", Environment.NewLine, CurrentTick, FPSCounter, FrameTime.ElapsedMilliseconds, GameApplication.Updater.TPSCounter);
            DebugInfo.Content = DebugInfoData;

            FrameTime.Reset();
            FrameTime.Start();
            if (CurrentTick == int.MaxValue) CurrentTick = 0;
            CurrentTick++;
        }

        private void UpdateDebugInfo(object sender, EventArgs e)
        {
            FPSCounter = CurrentTick - FPSLastTick;
            FPSLastTick = CurrentTick;
        }

        internal void StopRenderer()
        {
            DebugInfoUpdater.Stop();
            FrameUpdater.Stop();
        }

        internal void StartRenderer()
        {
            FrameUpdater.Start();
            FrameTime.Start();
            DebugInfoUpdater.Start();
        }
    }
}