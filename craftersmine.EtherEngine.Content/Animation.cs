using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.EtherEngine.Content
{
    /// <summary>
    /// Represents game object <see cref="Animation"/>
    /// </summary>
    public class Animation
    {
        private List<Image> frames = new List<Image>();

        /// <summary>
        /// Gets <see cref="Animation"/> texture
        /// </summary>
        public Texture AnimationTexture { get; internal set; }
        /// <summary>
        /// Gets total <see cref="Animation"/> frame count
        /// </summary>
        public int AnimationFramesCount { get; internal set; }
        /// <summary>
        /// Gets tick count that triggers next frame
        /// </summary>
        public int FrameTickTrigger { get; internal set; }
        /// <summary>
        /// Gets current <see cref="Animation"/> frame number
        /// </summary>
        public int CurrentFrame { get; set; }
        /// <summary>
        /// Gets or sets counted game ticks for trigger
        /// </summary>
        public int CountedTicks { get; set; }
        /// <summary>
        /// Gets width of one frame of <see cref="Animation"/>
        /// </summary>
        public int FrameWidth { get; internal set; }

        /// <summary>
        /// Creates new <see cref="Animation"/> instance
        /// </summary>
        /// <param name="texture"><see cref="Animation"/> not sliced by frames <see cref="Texture"/></param>
        /// <param name="frames">Total <see cref="Animation"/> frame count</param>
        /// <param name="frameTickTrigger">Sets trigger for what every tick count will <see cref="Animation"/> frame changed</param>
        /// <param name="frameWidth">Width of <see cref="Animation"/> frame</param>
        public Animation(Texture texture, int frames, int frameTickTrigger, int frameWidth)
        {
            AnimationTexture = texture;
            AnimationFramesCount = frames;
            FrameTickTrigger = frameTickTrigger;
            FrameWidth = frameWidth;
            PrepareAnimation();
        }

        /// <summary>
        /// Prepares <see cref="Animation"/> from <see cref="Animation.AnimationTexture"/>
        /// </summary>
        public void PrepareAnimation()
        {
            for (int i = 0; i < AnimationFramesCount; i++)
            {
                frames.Add(new Bitmap(FrameWidth, AnimationTexture.TextureImage.Height));
                var image = Graphics.FromImage(frames[i]);
                image.DrawImage(AnimationTexture.TextureImage, new Rectangle(0, 0, FrameWidth, AnimationTexture.TextureImage.Height), new Rectangle(i * FrameWidth, 0, FrameWidth, AnimationTexture.TextureImage.Height), GraphicsUnit.Pixel);
                image.Dispose();
            }
        }

        /// <summary>
        /// Gets frame <see cref="Image"/> from frame number
        /// </summary>
        /// <param name="frameId">Frame number</param>
        /// <returns>Frame <see cref="Image"/></returns>
        public Image GetFrame(int frameId)
        {
            return frames[frameId];
        }
    }
}
