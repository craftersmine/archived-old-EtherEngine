using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using craftersmine.EtherEngine.Utils;

namespace craftersmine.EtherEngine.Core
{
    /// <summary>
    /// Represents scene camera. This class cannot be inherited
    /// </summary>
    public sealed class Camera
    {
        /// <summary>
        /// Gets current camera global position by X axis
        /// </summary>
        public int X { get; internal set; }
        /// <summary>
        /// Gets current camera global position by Y axis
        /// </summary>
        public int Y { get; internal set; }
        /// <summary>
        /// Gets current camera width
        /// </summary>
        public int Width { get; internal set; }
        /// <summary>
        /// Gets current camera height
        /// </summary>
        public int Height { get; internal set; }
        /// <summary>
        /// Gets current camera bounds in <see cref="Rectangle">
        /// </summary>
        public Rectangle CameraBounds { get; internal set; }

        /// <summary>
        /// Creates new instance of camera with specified parameters
        /// </summary>
        /// <param name="x">Initial camera position by X axis</param>
        /// <param name="y">Initial camera position by Y axis</param>
        /// <param name="width">Viewport width</param>
        /// <param name="height">Viewport height</param>
        public Camera(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            CameraBounds = new Rectangle(X, Y, Width, Height);
        }

        /// <summary>
        /// Moves camera at specified vector
        /// </summary>
        /// <param name="vector">Vector data to move</param>
        public void MoveCamera(Vector2 vector)
        {
            X += vector.X;
            Y += vector.Y;
            if (GameApplication.GameWindow.CurrentScene != null)
                GameApplication.GameWindow.CurrentScene.UpdateCameraOffsettedPositions();
        }
        /// <summary>
        /// Updates camera bounding box rectangle. Use if you see graphics artifacts or nothing seem
        /// </summary>
        public void UpdateCameraBounds()
        {
            CameraBounds = new Rectangle(X, Y, Width, Height);
        }
    }
}
