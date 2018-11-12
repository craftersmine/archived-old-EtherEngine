using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using craftersmine.EtherEngine.Input;

namespace craftersmine.EtherEngine.Core
{
    public partial class GameWindow : Form
    {
        internal AcceleratedCanvas AcceleratedCanvas { get; set; }
        internal Dictionary<int, Scene> Scenes { get; set; } = new Dictionary<int, Scene>();

        private WindowSize _WindowSize;

        public WindowSize WindowSize { get { return _WindowSize; } set { _WindowSize = value; this.ClientSize = _WindowSize.GetSize(); } }
        public bool IsFrameVisible
        {
            get
            {
                switch (FormBorderStyle)
                {
                    case FormBorderStyle.None:
                        return false;
                    default:
                        return true;
                }
            }

            set
            {
                if (value)
                    FormBorderStyle = FormBorderStyle.FixedSingle;
                else FormBorderStyle = FormBorderStyle.None;
            }
        }
        public Scene CurrentScene { get; internal set; }

        public GameWindow(string title, WindowSize size)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            WindowSize = size;
            Text = title;
            GameApplication.Log(LogEntryType.Info, "Creating canvas...");
            AcceleratedCanvas = new AcceleratedCanvas();
            AcceleratedCanvas.Dock = DockStyle.Fill;
            this.Controls.Add(AcceleratedCanvas);
            this.Shown += WindowShown;
            this.FormClosing += WindowClosing;
            AcceleratedCanvas.Canvas._base.MouseUp += MouseUpHandler;
            AcceleratedCanvas.Canvas._base.MouseDown += MouseDownHandler;
            AcceleratedCanvas.Canvas._base.MouseMove += MouseHoverHandler;
            AcceleratedCanvas.Canvas._base.MouseWheel += MouseWheelHandler;
        }

        private void MouseWheelHandler(object sender, MouseWheelEventArgs e)
        {
            MouseWheelRotationType rotType = MouseWheelRotationType.NoRotation;
            if (e.Delta > 0)
                rotType = MouseWheelRotationType.FromUser;
            else if (e.Delta < 0)
                rotType = MouseWheelRotationType.ToUser;
            else rotType = MouseWheelRotationType.NoRotation;
            OnMouseWheel(new MouseWheelEventArguments { MouseWheelRotationType = rotType, XPosition = e.GetPosition(AcceleratedCanvas.Canvas._base).X, YPosition = e.GetPosition(AcceleratedCanvas.Canvas._base).Y });
        }

        private void MouseHoverHandler(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OnMouseHover(new MouseEventArguments { LeftButtonState = e.LeftButton, MiddleButtonState = e.MiddleButton, RightButtonState = e.RightButton, XPosition = e.GetPosition(AcceleratedCanvas.Canvas._base).X, YPosition = e.GetPosition(AcceleratedCanvas.Canvas._base).Y });
        }

        private void MouseDownHandler(object sender, MouseButtonEventArgs e)
        {
            OnMouseDown(new MouseEventArguments { LeftButtonState = e.LeftButton, MiddleButtonState = e.MiddleButton, RightButtonState = e.RightButton, XPosition = e.GetPosition(AcceleratedCanvas.Canvas._base).X, YPosition = e.GetPosition(AcceleratedCanvas.Canvas._base).Y });
        }

        private void MouseUpHandler(object sender, MouseButtonEventArgs e)
        {
            OnMouseUp(new MouseEventArguments { LeftButtonState = e.LeftButton, MiddleButtonState = e.MiddleButton, RightButtonState = e.RightButton, XPosition = e.GetPosition(AcceleratedCanvas.Canvas._base).X, YPosition = e.GetPosition(AcceleratedCanvas.Canvas._base).Y });
        }

        private void WindowClosing(object sender, FormClosingEventArgs e)
        {
            OnWindowClosing(e);
            if (!GameApplication.IsExiting)
            {
                GameApplication.Exit(0);
            }
        }

        public virtual void OnWindowClosing(FormClosingEventArgs e)
        { }

        private void WindowShown(object sender, EventArgs e)
        {
            GameApplication.Log(LogEntryType.Info, "Starting renderer...");
            GameApplication.Renderer.StartRenderer();
            GameApplication.Log(LogEntryType.Info, "Starting game updater...");
            GameApplication.Updater.StartUpdater();
            GameApplication.Log(LogEntryType.Done, "Game process initialized!");
            OnWindowShown();
        }

        public virtual void OnWindowShown()
        { }

        public void AddScene(int id, Scene scene)
        {
            Scenes.Add(id, scene);
        }

        public void RemoveScene(int id)
        {
            Scenes.Remove(id);
        }

        public void ShowScene(int id)
        {
            if (Scenes.ContainsKey(id))
            {
                CurrentScene?.OnSceneDestroyedInternal();
                CurrentScene = Scenes[id];
                GameApplication.Renderer.SceneBgBrush.Color = CurrentScene.BackgroundColor;
                GameApplication.Renderer.ClearScene();
                CurrentScene.OnSceneShownInternal();
            }
        }
        
        public void SetTextureInterpolation(BitmapScalingMode mode)
        {
            if (GameApplication.Renderer != null)
                GameApplication.Renderer.TextureInterpolation = mode;
            else throw new RendererException("Unable to change texture interpolation mode before renderer being initialized!");
        }
    }
}
