using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace craftersmine.EtherEngine.Core
{
    /// <summary>
    /// Represents collision box. This class cannot be inherited
    /// </summary>
    public sealed class CollisionBox
    {
        /// <summary>
        /// Gets collision box offset from object origin by X axis
        /// </summary>
        public double XOffset { get; internal set; }
        public double YOffset { get; internal set; }
        public double Height { get; internal set; }
        public double Width { get; internal set; }
        public Rect CollisionBoxBoundings { get; internal set; }

        public void UpdateCollisionBox(Transform objTransform)
        {
            CollisionBoxBoundings = new Rect(objTransform.RelativeCameraPosition.X + XOffset, objTransform.RelativeCameraPosition.Y + YOffset, Width, Height);
        }

        public void SetCollisionBox(double xOff, double yOff, double width, double height)
        {
            XOffset = xOff;
            YOffset = yOff;
            Width = width;
            Height = height;
        }
    }
}