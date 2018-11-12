using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace craftersmine.EtherEngine.Core
{
    public sealed class Viewport
    {
        internal Rect ViewportBoundings { get; set; }

        public Transform Transform { get; private set; }

        public Viewport(Transform transform)
        {
            UpdateViewport(transform);
        }

        public void UpdateViewport(Transform transform)
        {
            Transform = transform;
            ViewportBoundings = new Rect(Transform.Position.X, Transform.Position.Y, Transform.Bounds.Width, Transform.Bounds.Height);
        }
    }
}
