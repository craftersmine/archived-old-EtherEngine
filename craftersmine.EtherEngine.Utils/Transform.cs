using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.EtherEngine.Utils
{
    public sealed class Transform
    {
        public int X { get; internal set; }
        public int Y { get; internal set; }
        public int Height { get; internal set; }
        public int Width { get; internal set; }
        public int CameraX { get; internal set; }
        public int CameraY { get; internal set; }
        public Rectangle BoundingsRectangle { get; internal set; }
        public Rectangle CameraBoundings { get; internal set; }

        public Transform(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            BoundingsRectangle = new Rectangle(X, Y, Width, Height);
        }

        public void SetCameraPosition(int xCam, int yCam)
        {
            CameraX = X + xCam;
            CameraY = Y + yCam;
            CameraBoundings = new Rectangle(CameraX, CameraY, Height, Width);
        }

        public void Move(Vector2 vec)
        {
            X += vec.X;
            Y += vec.Y;
            Place(X, Y);
        }

        public void Inflate(int width, int height)
        {
            Width += width;
            Height += height;
            BoundingsRectangle.Inflate(width, height);
        }

        public void Place(int x, int y)
        {
            X = x; Y = y;
            BoundingsRectangle = new Rectangle(X, Y, Width, Height);
        }

        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
            BoundingsRectangle = new Rectangle(X, Y, Width, Height);
        }
    }
}
