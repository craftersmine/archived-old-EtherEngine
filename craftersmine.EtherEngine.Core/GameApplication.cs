using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using craftersmine.EtherEngine.Utils;

namespace craftersmine.EtherEngine.Core
{
    public sealed class GameApplication
    {
        public static bool IsProcessActive { get; internal set; }
        public static Logger Logger { get; internal set; }
        public static string AppDataPath { get; internal set; }

        internal static GameWindow GameWindow { get; set; }
        internal static Renderer Renderer { get; set; }
        internal static GameUpdater GameUpdater { get; set; }
        internal static WindowSize GameWindowSize { get; set; }
        internal static GameEngineLogViewer LogViewer { get; set; }

        public static void Run(GameWindow gameWindow, string appDataPath)
        {
            AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appDataPath);
            GameWindow = gameWindow;
            Renderer = new Renderer();
            GameUpdater = new GameUpdater();
            IsProcessActive = true;
            Renderer.DrawDebugInfo = true;
            Renderer.Run();
            GameUpdater.RunGameUpdater();
            GameWindow.OnCreate();
            Application.Run(GameWindow);
        }

        public static void Exit(int exitCode)
        {
            GameWindow.OnExit(exitCode);
            Logger?.Log(LogEntryType.Info, "Game exited! Exit code: " + exitCode);
            Environment.Exit(exitCode);
        }

        public static void SetLogger(string logsRoot, string logName)
        {
            Logger = new Logger(Path.Combine(AppDataPath, logsRoot), logName);
        }

        public static void Log(LogEntryType prefix, string contents, bool onlyConsole = false) => Logger?.Log(prefix, contents, onlyConsole);

        public static void LogException(LogEntryType prefix, Exception ex) => Logger?.LogException(prefix, ex);
    }
}
