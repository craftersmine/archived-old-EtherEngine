using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using craftersmine.GameEngine.Input;

namespace craftersmine.EtherEngine.Core
{
    public sealed class GameUpdater
    {
        private static Timer GameTickUpdater { get; set; }
        private static Timer GamePhysicsUpdater { get; set; }
        private static Timer TPSAndUPSTimer { get; set; }
        private static int Ticks { get; set; }
        private static int PhysicsUpdates { get; set; }
        private static int TPSLast { get; set; }
        private static int UPSLast { get; set; }

        internal static bool IsLogViewerVisible { get; set; }

        public static int TPS { get; set; }
        public static int UPS { get; set; }

        public void RunGameUpdater()
        {
            TPSAndUPSTimer = new Timer(new TimerCallback(TPSCalc), null, 0, 1000);
            GameTickUpdater = new Timer(new TimerCallback(GameTickUpdaterMethod), null, 0, 10);
            GamePhysicsUpdater = new Timer(new TimerCallback(GamePhysicsUpdaterMethod), null, 0, 10);
        }

        private void TPSCalc(object state)
        {
            TPS = Ticks - TPSLast;
            TPSLast = Ticks;
            UPS = PhysicsUpdates - UPSLast;
            UPSLast = PhysicsUpdates;
            GameApplication.Renderer.AdditionalDebugInfo = "TPS: " + TPS + "\r\nUPS: " + UPS;
        }

        private void GamePhysicsUpdaterMethod(object state)
        {
            if (GameApplication.GameWindow.CurrentScene != null)
                foreach(var layer in GameApplication.GameWindow.CurrentScene.GameLayers)
                {
                    for (int i = 0; i < layer.Value.GameObjects.Count; i++)
                    {
                        layer.Value.GameObjects[i].InternalPhysicsUpdate();
                        layer.Value.GameObjects[i].CheckCollisionFor(layer.Value.GameObjects);
                    }
                }
            PhysicsUpdates++;
        }

        private void GameTickUpdaterMethod(object state)
        {
            // Shift + Ctrl + F10 => Draws debug info
            if (Keyboard.IsKeyDown(System.Windows.Forms.Keys.ControlKey) && Keyboard.IsKeyDown(System.Windows.Forms.Keys.ShiftKey) && Keyboard.IsKeyDown(System.Windows.Forms.Keys.F10))
                GameApplication.Renderer.DrawDebugInfo = true;
            else GameApplication.Renderer.DrawDebugInfo = false;

            // Shift + Ctrl + F11 => Draws debug rectangles
            if (Keyboard.IsKeyDown(System.Windows.Forms.Keys.ControlKey) && Keyboard.IsKeyDown(System.Windows.Forms.Keys.ShiftKey) && Keyboard.IsKeyDown(System.Windows.Forms.Keys.F11))
                GameApplication.Renderer.DrawDebugRects = true;
            else GameApplication.Renderer.DrawDebugRects = false;

            // Shift + Ctrl + F12 => Shows coroutine debug
            //if (Keyboard.IsKeyDown(System.Windows.Forms.Keys.ControlKey) && Keyboard.IsKeyDown(System.Windows.Forms.Keys.ShiftKey) && Keyboard.IsKeyDown(System.Windows.Forms.Keys.F12) && !IsLogViewerVisible)
            //    ShowLogViewer();

            
            GameApplication.GameWindow.OnUpdate();
            if (GameApplication.GameWindow.CurrentScene != null)
            {
                GameApplication.GameWindow.CurrentScene.OnUpdate();
                foreach (var layer in GameApplication.GameWindow.CurrentScene.GameLayers)
                {
                    for (int gObjIndex = 0; gObjIndex < layer.Value.GameObjects.Count; gObjIndex++)
                    {
                        layer.Value.GameObjects[gObjIndex].OnUpdate();
                        layer.Value.GameObjects[gObjIndex].InternalUpdate();
                    }
                }
            }
            Ticks++;
        }

        //private void ShowLogViewer()
        //{
        //    if (!IsLogViewerVisible)
        //    {
        //        GameApplication.LogViewer?.Show();
        //        IsLogViewerVisible = true;
        //    }
        //}
    }
}
