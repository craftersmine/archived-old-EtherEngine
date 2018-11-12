using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using craftersmine.EtherEngine.Content;

namespace craftersmine.EtherEngine.Core
{
    public sealed class GameApplication
    {
        internal static Renderer Renderer { get; set; }
        internal static GameWindow GameWnd { get; set; }
        internal static GameUpdater Updater { get; set; }
        internal static string TemporaryDirectory { get; private set; }
        internal static ContentStorage InternalResources { get; set; }
        internal static Dictionary<string, Texture> InternalTextures { get; set; } = new Dictionary<string, Texture>();
        internal static List<Coroutine> RegisteredCoroutines { get; set; } = new List<Coroutine>();
        
        public static Logger Logger { get; private set; }
        public static bool IsExiting { get; internal set; }

        public static void Run(GameWindow window)
        {
            try
            {
                TemporaryDirectory = Environment.GetEnvironmentVariable("temp");
                SetLogger(new Logger(TemporaryDirectory, "craftersmine_EtherEngine_Game_" + Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location)));

                Log(LogEntryType.Info, "Initializing game process...");
                GameWnd = window;
                Log(LogEntryType.Info, "Loading internal engine resources...");
                InternalResources = new ContentStorage("engine.internal");
                string[] meta = InternalResources.LoadStrings("metadata");
                foreach (var lineMeta in meta)
                {
                    string[] nameType = lineMeta.Split('=');
                    if (nameType.Length > 0)
                    {
                        switch (nameType[1].ToLower())
                        {
                            case "texture":
                                Log(LogEntryType.Info, "InternalContentLoader: Loading texture: " + nameType[0]);
                                InternalTextures.Add(nameType[0], InternalResources.LoadTexture(nameType[0], TextureLayout.Tile));
                                break;
                            default:
                                Log(LogEntryType.Warning, "InternalContentLoader: Invalid type of resource \"" + nameType[0] + "\"! Skipping it!");
                                break;
                        }
                    }
                }
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
            Log(LogEntryType.Info, "Stopping registered coroutines...");
            if (RegisteredCoroutines.Count > 0)
                for (int i = 0; i < RegisteredCoroutines.Count; i++)
                    RegisteredCoroutines[i].StopCoroutine();
            if (RegisteredCoroutines.Count == 0)
                Log(LogEntryType.Info, "All coroutines has stopped or no running coroutines exist!");
            else Log(LogEntryType.Warning, "Some coroutines unable to stop! Skipping...");
            Log(LogEntryType.Info, "Stopping game updater thread...");
            if (Updater != null)
                Updater.StopUpdater();
            else Log(LogEntryType.Warning, "No game updater instance found! Game not started yet?");
            Log(LogEntryType.Info, "Stopping renderer thread...");
            if (Renderer != null)
                Renderer.StopRenderer();
            else Log(LogEntryType.Warning, "No game renderer instance found! Game not started yet?");
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
