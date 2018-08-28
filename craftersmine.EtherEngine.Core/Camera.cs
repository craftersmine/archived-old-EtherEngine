using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using craftersmine.EtherEngine.Utils;

namespace craftersmine.EtherEngine.Core
{
    public sealed class Camera
    {
        public int X { get; internal set; }
        public int Y { get; internal set; }
        public int Width { get; internal set; }
        public int Height { get; internal set; }
        public Rectangle CameraBounds { get; internal set; }

        public Camera(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            CameraBounds = new Rectangle(X, Y, Width, Height);
        }

        public void MoveCamera(Vector2 vector)
        {
            X += vector.X;
            Y += vector.Y;
            if (GameApplication.GameWindow.CurrentScene != null)
                GameApplication.GameWindow.CurrentScene.UpdateCameraOffsettedPositions();
        }

        public void UpdateCameraBounds()
        {
            CameraBounds = new Rectangle(X, Y, Width, Height);
        }
    }
}
