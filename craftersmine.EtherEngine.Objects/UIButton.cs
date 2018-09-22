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
    /// <summary>
    /// Represents UI Button
    /// </summary>
    public class UIButton : UIControl
    {
        private Texture UsedTexture { get; set; }
        private int ButtonHeight { get; set; }
        private bool IsMouseHover { get; set; }
        /// <summary>
        /// Gets or sets the button reacts on click event
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Creates new UI button instance with specified size and texture
        /// </summary>
        /// <param name="width">Width of button</param>
        /// <param name="height">Height of button</param>
        /// <param name="texture">Button texture</param>
        /// <param name="buttonHeight">Button state image height in texture</param>
        public UIButton(int width, int height, Texture texture, int buttonHeight) : base(width, height)
        {
            UsedTexture = texture;
            ButtonHeight = buttonHeight;
        }

        /// <summary>
        /// Calls on render call. Use to define how button draws
        /// </summary>
        /// <param name="graphics">Painter</param>
        public override void DrawMethod(Graphics graphics)
        {
            base.DrawMethod(graphics);
            if (IsMouseHover)
                graphics.DrawImage(UsedTexture.TextureImage, new Rectangle(this.Transform.X, this.Transform.Y, this.Transform.Width, this.Transform.Height), new Rectangle(0, ButtonHeight, UsedTexture.TextureImage.Width, ButtonHeight), GraphicsUnit.Pixel);
            IsMouseHover = false;
        }

        /// <summary>
        /// Calls on mouse click on button
        /// </summary>
        /// <param name="button">Mouse button</param>
        /// <param name="x">Global position of click by X axis</param>
        /// <param name="y">Global position of click by Y axis</param>
        public override void OnMouseClick(MouseButtons button, int x, int y)
        {
            base.OnMouseClick(button, x, y);
        }

        /// <summary>
        /// Calls when mouse hovering on button
        /// </summary>
        /// <param name="xMousePos">Global position of mouse position by X axis</param>
        /// <param name="yMousePos">Global position of mouse position by Y axis</param>
        public override void OnMouseHover(int xMousePos, int yMousePos)
        {
            base.OnMouseHover(xMousePos, yMousePos);
            IsMouseHover = true;
        }
    }
}
