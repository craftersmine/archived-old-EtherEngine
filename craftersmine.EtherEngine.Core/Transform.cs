using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace craftersmine.EtherEngine.Core
{
    public sealed class Transform
    {
        public Rect Bounds { get; set; }
        public Vector2 Position { get; set; }

        internal Vector2 RelativeCameraPosition { get; set; }

        public Transform(Rect bounds, Vector2 position)
        {
            Bounds = bounds;
            Position = position;
        }

        public Transform(Rect bounds) : this(bounds, Vector2.Empty)
        {}

        internal void UpdateCoordsRelativeViewport(Transform cameraTransform)
        {
            RelativeCameraPosition = Position + cameraTransform.Position;
        }
    }
}
