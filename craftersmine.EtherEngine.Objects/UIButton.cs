using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using craftersmine.EtherEngine.Content;
using craftersmine.EtherEngine.Core;

namespace craftersmine.EtherEngine.Objects
{
    public sealed class UIButton : UIControl
    {
        private Texture UsedTexture { get; set; }
        private int ButtonHeight { get; set; }
        private bool IsMouseHover { get; set; }
        public bool IsEnabled { get; set; }

        public UIButton(int width, int height, Texture texture, int buttonHeight) : base(width, height)
        {
            UsedTexture = texture;
            ButtonHeight = buttonHeight;
        }

        public override void DrawMethod(Graphics graphics)
        {
            base.DrawMethod(graphics);
            if (IsMouseHover)
                graphics.DrawImage(UsedTexture.TextureImage, new Rectangle(this.Transform.X, this.Transform.Y, this.Transform.Width, this.Transform.Height), new Rectangle(0, ButtonHeight, UsedTexture.TextureImage.Width, ButtonHeight), GraphicsUnit.Pixel);
            IsMouseHover = false;
        }

        public override void OnMouseClick(MouseButtons button, int x, int y)
        {
            base.OnMouseClick(button, x, y);
        }

        public override void OnMouseHover(int xMousePos, int yMousePos)
        {
            base.OnMouseHover(xMousePos, yMousePos);
            IsMouseHover = true;
        }
    }
}
