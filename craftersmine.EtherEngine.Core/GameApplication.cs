using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace craftersmine.EtherEngine.Core
{
    public sealed class GameApplication
    {
        internal static Renderer Renderer { get; set; }
        internal static GameWindow GameWnd { get; set; }
        internal static GameUpdater Updater { get; set; }
        internal static string TemporaryDirectory { get; private set; }
        
        public static Logger Logger { get; private set; }
        public static bool IsExiting { get; internal set; }

        public static void Run(GameWindow window)
        {
            try
            {
                TemporaryDirectory = Environment.GetEnvironmentVariable("temp");
                SetLogger(new Logger(TemporaryDirectory, "craftersmine_EtherEngine_Game_" + Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location)));


                Log(LogEntryType.Info, "Initializing game process...");
                GameWnd = window;
                Log(LogEntryType.Info, "Creating renderer...");
                Renderer = new Renderer(GameWnd.AcceleratedCanvas);
                Log(LogEntryType.Info, "Creating game updater...");
                Updater = new GameUpdater();
                //GameWnd.AcceleratedCanvas.AttachRenderer(Renderer);
                Log(LogEntryType.Info, "Initializing window...");
                Application.Run(GameWnd);
            }
            catch (Exception ex)
            {
                Log(LogEntryType.Info, "Unhandled exception is occured! Process will be terminated!");
                LogException(LogEntryType.Crash, ex);
                Exit(ex.HResult);
            }
        }

        public static void Exit(int exitCode)
        {
            Log(LogEntryType.Info, "Exiting game...");
            Log(LogEntryType.Info, "Stopping game updater thread...");
            Updater.StopUpdater();
            Log(LogEntryType.Info, "Stopping renderer thread...");
            Renderer.StopRenderer();
            Log(LogEntryType.Info, "Game exited! Exit code: " + exitCode + " (0x" + exitCode.ToString("X") + ")");
            Environment.Exit(exitCode);
        }

        public static void SetLogger(Logger logger)
        {
            Logger = logger;
        }

        public static void Log(LogEntryType prefix, string contents, bool onlyConsole = false)
        {
            Logger?.Log(prefix, contents, onlyConsole);
        }

        public static void LogToConsole(LogEntryType prefix, string contents)
        {
            Log(prefix, contents, true);
        }

        public static void LogException(LogEntryType prefix, Exception exception)
        {
            Logger?.LogException(prefix, exception);
        }
    }
}
