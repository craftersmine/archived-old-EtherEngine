using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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

        public BitmapImage RendererInternalTexture { get; private set; }

        /// <summary>
        /// Creates new <see cref="Texture"/> instance with <paramref name="textureImage"/>
        /// </summary>
        /// <param name="textureImage"><see cref="Image"/> for <see cref="Texture"/></param>
        /// <param name="textureLayout">Sets texture layout</param>
        public Texture(Image textureImage, TextureLayout textureLayout)
        {
            UpdateTexture(textureImage, textureLayout);

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
            RendererInternalTexture = BitmapToImageSource(TextureImage);
        }

        /// <summary>
        /// Loads texture from file with specified texture layout
        /// </summary>
        /// <param name="filepath">Filepath to texture</param>
        /// <param name="textureLayout">Texture layout</param>
        /// <returns></returns>
        public static Texture FromFile(string filepath, TextureLayout textureLayout)
        {
            try
            {
                if (filepath == null)
                    throw new ArgumentNullException(nameof(filepath));
                Image texImg = Image.FromFile(filepath);
                Texture texture = new Texture(texImg, textureLayout);
                return texture;
            }
            catch (Exception ex)
            {
                throw new ContentLoadException("Unable to load texture from " + filepath + "! Inner exception message: " + ex.Message, ex);
            }
        }

        private BitmapImage BitmapToImageSource(Image bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
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
