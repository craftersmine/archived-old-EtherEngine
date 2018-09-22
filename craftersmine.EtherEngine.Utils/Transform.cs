using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.EtherEngine.Utils
{
    /// <summary>
    /// Represents object transformation methods. This class cannot be inherited
    /// </summary>
    public sealed class Transform
    {
        /// <summary>
        /// Gets global position of object by X axis
        /// </summary>
        public int X { get; internal set; }
        /// <summary>
        /// Gets global position of object by Y axis
        /// </summary>
        public int Y { get; internal set; }
        /// <summary>
        /// Gets height of object
        /// </summary>
        public int Height { get; internal set; }
        /// <summary>
        /// Gets width of object
        /// </summary>
        public int Width { get; internal set; }
        /// <summary>
        /// Gets position of object by X axis relative to camera
        /// </summary>
        public int CameraX { get; internal set; }
        /// <summary>
        /// Gets position of object by Y axis relative to camera
        /// </summary>
        public int CameraY { get; internal set; }
        /// <summary>
        /// Gets object boundings rectangle
        /// </summary>
        public Rectangle BoundingsRectangle { get; internal set; }
        /// <summary>
        /// Gets object boundings rectangle relative to camera
        /// </summary>
        public Rectangle CameraBoundings { get; internal set; }

        /// <summary>
        /// Creates new instance of <see cref="Transform"/>
        /// </summary>
        /// <param name="x">Global position of object by X axis</param>
        /// <param name="y">Global position of object by Y axis</param>
        /// <param name="width">Width of object</param>
        /// <param name="height">Height of object</param>
        public Transform(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            BoundingsRectangle = new Rectangle(X, Y, Width, Height);
        }

        /// <summary>
        /// Sets object position relative to camera
        /// </summary>
        /// <param name="xCam">Position of object by X axis relative to camera</param>
        /// <param name="yCam">Position of object by Y axis relative to camera</param>
        public void SetCameraPosition(int xCam, int yCam)
        {
            CameraX = X + xCam;
            CameraY = Y + yCam;
            CameraBoundings = new Rectangle(CameraX, CameraY, Height, Width);
        }

        /// <summary>
        /// Moves object on specified <see cref="Vector2"/>
        /// </summary>
        /// <param name="vec">Vector to move</param>
        public void Move(Vector2 vec)
        {
            X += vec.X;
            Y += vec.Y;
            Place(X, Y);
        }

        /// <summary>
        /// Inflates object boundings rectangle on specified width and height
        /// </summary>
        /// <param name="width">Width to inflate</param>
        /// <param name="height">Height to inflate</param>
        public void Inflate(int width, int height)
        {
            Width += width;
            Height += height;
            BoundingsRectangle.Inflate(width, height);
        }

        /// <summary>
        /// Places object to specified position in world
        /// </summary>
        /// <param name="x">Position of object by X axis</param>
        /// <param name="y">Position of object by Y axis</param>
        public void Place(int x, int y)
        {
            X = x; Y = y;
            BoundingsRectangle = new Rectangle(X, Y, Width, Height);
        }

        /// <summary>
        /// Sets size of object to specified size
        /// </summary>
        /// <param name="width">Width of object</param>
        /// <param name="height">Height of object</param>
        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
            BoundingsRectangle = new Rectangle(X, Y, Width, Height);
        }
    }
}
