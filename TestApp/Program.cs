using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using craftersmine.EtherEngine.Core;
using craftersmine.EtherEngine.Input;

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
            AddScene(0, new Scene1());
            ShowScene(0);
        }
    }

    public class Scene1 : Scene
    {
        public Coroutine coroutine;

        public override void OnShown()
        {
            coroutine = new Coroutine(new CoroutineCallback(() => { Console.WriteLine("Coroutine is up!"); }));
            //RegisterCoroutine(coroutine);
        }
    }
}
