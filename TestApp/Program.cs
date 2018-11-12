using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using craftersmine.EtherEngine.Core;
using craftersmine.EtherEngine.Input;
using craftersmine.GameEngine.Input;

namespace TestApp
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Wnd wnd = new Wnd();
            GameApplication.Run(wnd);
        }
    }

    public class Wnd : GameWindow
    {
        public Wnd() : base ("TestApp", new WindowSize(800, 600))
        {
        }

        public override void OnMouseUp(MouseEventArguments args)
        {
            GameApplication.LogToConsole(LogEntryType.Info, args.ToString());
        }

        public override void OnWindowShown()
        {
            SetTextureInterpolation(BitmapScalingMode.NearestNeighbor);
            AddScene(0, new Scene1());
            ShowScene(0);
        }
    }

    public class Scene1 : Scene
    {
        public Coroutine coroutine;

        public override void OnShown()
        {
            BackgroundColor = Colors.Black;
            GO testGO = new GO();
            //GO1 testGO1 = new GO1();
            AddGameObject(testGO);
        }

        public override void OnUpdate()
        {
            if (Keyboard.IsKeyDown(Keys.A))
                Viewport.Transform.Position += new Vector2(-1);
        }
    }

    public class GO : GameObject
    {
        int i = 0;
        public override void OnUpdate()
        {
            if (Keyboard.IsKeyDown(Keys.D))
                i++;
            else { i = 0; }
            this.Transform.Position += new Vector2(i);
        }
    }
    public class GO1 : GameObject
    {
        public override void OnCreated()
        {
            this.Transform.Position = new Vector2(128, 128);
            this.Transform.Bounds = new Rect(0, 0, 128, 128);
        }
    }
}
