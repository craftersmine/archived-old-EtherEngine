using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.EtherEngine.Core
{
    public sealed class Transform
    {
        private Vector2 _pos;
        public Rectangle Bounds { get; set; }
        public Vector2 Position { get { return _pos; } set { _pos = value; CameraX += _pos.X; CameraY += _pos.Y; } }

        internal double CameraX { get; set; }
        internal double CameraY { get; set; }

        public Transform(Rectangle bounds, Vector2 position)
        {
            Bounds = bounds;
            Position = position;
        }

        public Transform(Rectangle bounds) : this(bounds, Vector2.Empty)
        {}
    }
}
