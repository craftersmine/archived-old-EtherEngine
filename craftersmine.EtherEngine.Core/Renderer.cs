using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using craftersmine.GameEngine.Input;

namespace craftersmine.EtherEngine.Core
{
    public sealed class Renderer
    {
        private string DebugInfoData { get; set; }
        private int FPSLastTick { get; set; }

        internal SolidColorBrush SceneBgBrush { get; set; } = new SolidColorBrush(Colors.Black);
        internal SolidColorBrush GameObjBoundingsBrush { get; set; } = new SolidColorBrush(Colors.Yellow);
        internal AccurateTimer FrameUpdater { get; private set; }
        internal DispatcherTimer DebugInfoUpdater { get; private set; }
        internal AcceleratedCanvas AcceleratedCanvas { get; set; }
        internal Stopwatch FrameTime { get; private set; }

        internal Label DebugInfo { get; set; } = new Label() { Foreground = new SolidColorBrush(Color.FromRgb(0, 255,0)), Content = "" };
        internal Grid SceneGrid { get; private set; }

        internal int CurrentTick { get; private set; }
        internal int FPSCounter { get; private set; }
        internal BitmapScalingMode TextureInterpolation { get; set; }

        public Renderer(AcceleratedCanvas acceleratedCanvas)
        {
            TextureInterpolation = BitmapScalingMode.Linear;
            AcceleratedCanvas = acceleratedCanvas;
            FrameUpdater = new AccurateTimer(new Action(Render), 17);
            DebugInfoUpdater = new DispatcherTimer();
            DebugInfoUpdater.Interval = TimeSpan.FromSeconds(1d);
            DebugInfoUpdater.Tick += UpdateDebugInfo;

            FrameTime = new Stopwatch();
            SceneGrid = new Grid();
            SceneGrid.Background = SceneBgBrush;
            AcceleratedCanvas.Canvas._base.Children.Add(SceneGrid);
            AcceleratedCanvas.Canvas._base.Children.Add(DebugInfo);
        }

        private void Render()
        {
            DebugInfoData = string.Format("craftersmine EtherEngine - Game debug info{0}{0}  Draw Call: {1}{0}  FPS: {2}{0}  Frame Time: {3} ms{0}  TPS: {4}", Environment.NewLine, CurrentTick, FPSCounter, FrameTime.ElapsedMilliseconds, GameApplication.Updater.TPSCounter);
            DebugInfo.Content = DebugInfoData;
            if (GameApplication.GameWnd.CurrentScene != null)
            {
                SceneBgBrush.Color = GameApplication.GameWnd.CurrentScene.BackgroundColor;
                SceneGrid.Background = SceneBgBrush;
                if (GameApplication.GameWnd.CurrentScene.GameObjects.Count > 0)
                {
                    for (int gObj = 0; gObj < GameApplication.GameWnd.CurrentScene.GameObjects.Count; gObj++)
                    {
                        //GameApplication.Log(LogEntryType.Debug, GameApplication.GameWnd.CurrentScene.GameObjects[gObj].InternalGameName + "    X: " + GameApplication.GameWnd.CurrentScene.GameObjects[gObj].Transform.Position.X + " | Y: " + GameApplication.GameWnd.CurrentScene.GameObjects[gObj].Transform.Position.Y + " | CamX: " + GameApplication.GameWnd.CurrentScene.GameObjects[gObj].Transform.RelativeCameraPosition.X + " | CamY: " + GameApplication.GameWnd.CurrentScene.GameObjects[gObj].Transform.RelativeCameraPosition.Y, true);
                        GameApplication.GameWnd.CurrentScene.GameObjects[gObj].GameObjectBase.Margin = new Thickness(GameApplication.GameWnd.CurrentScene.GameObjects[gObj].Transform.RelativeCameraPosition.X, GameApplication.GameWnd.CurrentScene.GameObjects[gObj].GameObjectBase.Margin.Top, GameApplication.GameWnd.CurrentScene.GameObjects[gObj].GameObjectBase.Margin.Right, GameApplication.GameWnd.CurrentScene.GameObjects[gObj].Transform.RelativeCameraPosition.Y);
                        GameApplication.GameWnd.CurrentScene.GameObjects[gObj].GameObjectBase.Stroke = GameObjBoundingsBrush;


                        GameApplication.GameWnd.CurrentScene.GameObjects[gObj].GameObjectBase.Fill = new ImageBrush(GameApplication.GameWnd.CurrentScene.GameObjects[gObj].CurrentTexture.RendererInternalTexture);

                        RenderOptions.SetBitmapScalingMode(GameApplication.GameWnd.CurrentScene.GameObjects[gObj].GameObjectBase.Fill, TextureInterpolation);
                        switch (GameApplication.GameWnd.CurrentScene.GameObjects[gObj].CurrentTexture.TextureLayout)
                        {
                            case Content.TextureLayout.Default:
                            case Content.TextureLayout.Stretch:
                                GameApplication.GameWnd.CurrentScene.GameObjects[gObj].GameObjectBase.Fill = new ImageBrush(GameApplication.GameWnd.CurrentScene.GameObjects[gObj].CurrentTexture.RendererInternalTexture) { Stretch = Stretch.Fill, TileMode = TileMode.None };
                                break;
                            case Content.TextureLayout.Tile:
                                GameApplication.GameWnd.CurrentScene.GameObjects[gObj].GameObjectBase.Fill = new ImageBrush(GameApplication.GameWnd.CurrentScene.GameObjects[gObj].CurrentTexture.RendererInternalTexture) { Stretch = Stretch.Fill, TileMode = TileMode.Tile, ViewportUnits = BrushMappingMode.Absolute, Viewport = new Rect(0d, 0d, GameApplication.GameWnd.CurrentScene.GameObjects[gObj].CurrentTexture.RendererInternalTexture.PixelWidth, GameApplication.GameWnd.CurrentScene.GameObjects[gObj].CurrentTexture.RendererInternalTexture.PixelHeight) };
                                break;
                        }


                        if (!GameApplication.GameWnd.CurrentScene.GameObjects[gObj].IsRendererProcessed)
                        {
                            SceneGrid.Children.Add(GameApplication.GameWnd.CurrentScene.GameObjects[gObj].GameObjectBase);
                            GameApplication.GameWnd.CurrentScene.GameObjects[gObj].IsRendererProcessed = true;
                        }
                    }
                }
            }
            FrameTime.Reset();
            FrameTime.Start();
            if (CurrentTick == int.MaxValue) CurrentTick = 0;
            CurrentTick++;
        }

        internal void ClearScene()
        {
            SceneGrid.Children.Clear();
            for (int gObj = 0; gObj < GameApplication.GameWnd.CurrentScene.GameObjects.Count; gObj++)
            {
                GameApplication.GameWnd.CurrentScene.GameObjects[gObj].IsRendererProcessed = true;
            }
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