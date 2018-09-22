using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RazorGDI.Core;
using RazorGDI;
using System.Drawing.Drawing2D;
using craftersmine.GameEngine.Input;

namespace craftersmine.EtherEngine.Core
{
    public class GameWindow : Form
    {
        private SizeF ScaleRatio = new SizeF(1.0f, 1.0f);
        private Dictionary<int, Scene> Scenes { get; set; } = new Dictionary<int, Scene>();
        internal RazorPainterControl Canvas { get; set; }
        internal Color BaseColor { get; set; }
        internal Scene CurrentScene { get; set; }
        internal Rectangle MousePoint { get; set; } 
        public string Title { get { return this.Text; } set { this.Text = value; } }

        public bool IsFramed { get
            {
                if (FormBorderStyle == FormBorderStyle.FixedSingle)
                    return true;
                else if (FormBorderStyle == FormBorderStyle.None)
                    return false;
                else return true;
            }
            set
            {
                if (value == true)
                    FormBorderStyle = FormBorderStyle.FixedSingle;
                else FormBorderStyle = FormBorderStyle.None;
            }
        }

        public GameWindow(string title, WindowSize windowSize, bool isFramed)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Title = title;
            MousePoint = new Rectangle(0, 0, 1, 1);
            //this.SetStyle(ControlStyles.UserPaint)
            this.FormClosing += WindowClosing;
            // Set size of window
            this.Size = windowSize.GetSize();
            GameApplication.GameWindowSize = windowSize;
            // Create canvas control
            Canvas = new RazorPainterControl();
            Canvas.Size = this.Size;
            Canvas.MouseClick += Canvas_MouseClick;
            Canvas.MouseMove += Canvas_MouseMove;
            Canvas.KeyUp += Canvas_KeyUp;
            Canvas.KeyDown += Canvas_KeyDown;
            this.Controls.Add(Canvas);
            // Set frame
            IsFramed = isFramed;
            // Prepare canvas
            BaseColor = Color.LightSkyBlue;
            Canvas.RazorGFX.Clear(BaseColor);
            Canvas.RazorPaint();
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            Keyboard.UpdateKeys(e.KeyData, e.Modifiers);
        }

        private void Canvas_KeyUp(object sender, KeyEventArgs e)
        {
            Keyboard.UpdateKeys(Keys.None, Keys.None);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            MousePoint = new Rectangle(e.X, e.Y, 1, 1);
            if (this.CurrentScene != null)
            {
                for (int i = 0; i < this.CurrentScene.GameObjects.Count; i++)
                {
                    if (MousePoint.IntersectsWith(this.CurrentScene.GameObjects[i].Transform.CameraBoundings))
                        this.CurrentScene.GameObjects[i].OnMouseHover(MousePoint.X, MousePoint.Y);
                }
                for (int u = 0; u < this.CurrentScene.UIControls.Count; u++)
                {
                    if (MousePoint.IntersectsWith(this.CurrentScene.UIControls[u].Transform.BoundingsRectangle))
                        this.CurrentScene.UIControls[u].OnMouseHover(MousePoint.X, MousePoint.Y);
                }
            }
        }

        private void Canvas_MouseClick(object sender, MouseEventArgs e)
        {
            MousePoint = new Rectangle(e.X, e.Y, 1, 1);
            if (this.CurrentScene != null)
            {
                for (int i = 0; i < this.CurrentScene.GameObjects.Count; i++)
                {
                    if (MousePoint.IntersectsWith(this.CurrentScene.GameObjects[i].Transform.CameraBoundings))
                        this.CurrentScene.GameObjects[i].OnMouseClick(e.Button, MousePoint.X, MousePoint.Y);
                }
                for (int u = 0; u < this.CurrentScene.UIControls.Count; u++)
                {
                    if (MousePoint.IntersectsWith(this.CurrentScene.UIControls[u].Transform.BoundingsRectangle))
                        this.CurrentScene.UIControls[u].OnMouseClick(e.Button, MousePoint.X, MousePoint.Y);
                }
            }
        }

        private void WindowClosing(object sender, FormClosingEventArgs e)
        {
            GameApplication.Exit(0);
        }

        public void SetScene(int sceneId)
        {
            CurrentScene = Scenes[sceneId];
        }

        public void AddScene(int id, Scene scene)
        {
            Scenes.Add(id, scene);
            scene.Camera.Width = GameApplication.GameWindowSize.Width;
            scene.Camera.Height = GameApplication.GameWindowSize.Height;
            scene.Camera.UpdateCameraBounds();
            scene.OnCreate();
        }

        public void RemoveScene(int sceneId)
        {
            if (Scenes.ContainsKey(sceneId))
            {
                Scenes.Remove(sceneId);
            }
        }

        public void SetTextureInterpolationMode(InterpolationMode interpolationMode)
        {
            GameApplication.Renderer.TextureInterpolationMode = interpolationMode;
        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnExit(int exitCode)
        {

        }

        public virtual void OnCreate()
        {
            
        }

        //public new void Scale(float ratio)
        //{
        //    ScaleRatio.Height = ratio;
        //    ScaleRatio.Width = ratio;
        //    Scale(ScaleRatio);
        //}
    }
}
