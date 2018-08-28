using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using craftersmine.EtherEngine.Utils;

namespace craftersmine.EtherEngine.Core
{
    public class UIControl
    {
        public Transform Transform { get; internal set; }
        internal Bitmap DrawableImage { get; set; }
        internal Graphics Painter { get; set; }
        public int Width { get { return Transform.Width; } }
        public int Height { get { return Transform.Height; } }

        public UIControl(int width, int height)
        {
            Transform = new Transform(0, 0, width, height);
            DrawableImage = new Bitmap(width, height);
            Transform.SetSize(width, height);
            Painter = Graphics.FromImage(DrawableImage);
            Painter.Clear(Color.Transparent);
        }

        public void SetPosition(int x, int y)
        {
            Transform.Place(x, y);
        }

        public virtual void OnMouseClick(MouseButtons button, int x, int y)
        {

        }
        
        public virtual void OnMouseHover(int xMousePos, int yMousePos)
        {

        }

        public virtual void DrawMethod(Graphics graphics)
        {

        }

        internal void CallDrawMethod()
        {
            DrawMethod(Painter);
        }
    }
}
