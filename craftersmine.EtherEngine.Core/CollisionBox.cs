using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int XOffset { get; internal set; }
        public int YOffset { get; internal set; }
        public int Height { get; internal set; }
        public int Width { get; internal set; }
        public Rectangle CollisionBoxBoundings { get; internal set; }

        public void UpdateCollisionBox(Transform objTransform)
        {
            CollisionBoxBoundings = new Rectangle((int)objTransform.CameraX + XOffset, (int)objTransform.CameraY + YOffset, Width, Height);
        }

        public void SetCollisionBox(int xOff, int yOff, int width, int height)
        {
            XOffset = xOff;
            YOffset = yOff;
            Width = width;
            Height = height;
        }
    }
}