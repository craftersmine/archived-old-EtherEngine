using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.EtherEngine.Content
{
    /// <summary>
    /// Represents texture
    /// </summary>
    public class Texture
    {
        /// <summary>
        /// Gets <see cref="Texture"/> image
        /// </summary>
        public Image TextureImage { get; private set; }

        /// <summary>
        /// Gets used texture layout
        /// </summary>
        public TextureLayout TextureLayout { get; private set; }

        /// <summary>
        /// Creates new <see cref="Texture"/> instance with <paramref name="textureImage"/>
        /// </summary>
        /// <param name="textureImage"><see cref="Image"/> for <see cref="Texture"/></param>
        /// <param name="textureLayout">Sets texture layout</param>
        public Texture(Image textureImage, TextureLayout textureLayout)
        {
            TextureImage = textureImage;
            TextureLayout = textureLayout;
        }

        /// <summary>
        /// Updates <see cref="Texture"/> image
        /// </summary>
        /// <param name="textureImage"><see cref="Image"/> for <see cref="Texture"/></param>
        /// <param name="textureLayout">Sets texture layout</param>
        public void UpdateTexture(Image textureImage, TextureLayout textureLayout)
        {
            TextureImage = textureImage;
            TextureLayout = textureLayout;
        }

			 public static Texture FromFile(string filepath, TextureLayout textureLayout)
			 {
            if (filepath == null)
                throw new ArgumentNullException(nameof(filepath));
            Image texImg = Image.FromFile(filepath);
            Texture texture = new Texture(texImg, textureLayout);
            return texture;
			 }
    }

    /// <summary>
    /// Texture layout types
    /// </summary>
    public enum TextureLayout
    {
        /// <summary>
        /// Stretch by default
        /// </summary>
        Default,
        /// <summary>
        /// Stretches texture on full size of object
        /// </summary>
        Stretch, 
        /// <summary>
        /// Tiles texture on object
        /// </summary>
        Tile,
        /// <summary>
        /// Center texture on object center
        /// </summary>
        Center
    }
}
