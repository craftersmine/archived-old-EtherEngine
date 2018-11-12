using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;

namespace craftersmine.EtherEngine.Content
{
    /// <summary>
    /// Represents game content storage
    /// </summary>
    public sealed class ContentStorage
    {
        private string PackagePath { get; set; }
        /// <summary>
        /// Gets content package name
        /// </summary>
        public string PackageName { get; internal set; }

        /// <summary>
        /// Creates new <see cref="ContentStorage"/> instance with <paramref name="packageName"/>
        /// </summary>
        /// <param name="packageName">Package name without extention from "content" root game directory</param>
        public ContentStorage(string packageName)
        {
            PackageName = packageName;
            PackageName = PackageName.Replace('/', Path.DirectorySeparatorChar).Replace('.', Path.DirectorySeparatorChar);
            CreateContentStorage();
        }
        
        private void CreateContentStorage()
        {
            PackagePath = Path.Combine(Environment.CurrentDirectory, "content", PackageName + ".etp");
            ContentStorageCreated?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Loads <see cref="Texture"/> from package
        /// </summary>
        /// <param name="name">Name of <see cref="Texture"/></param>
        /// <param name="textureLayout">Layout of texture</param>
        /// <returns><see cref="Texture"/></returns>
        public Texture LoadTexture(string name, TextureLayout textureLayout)
        {
            ContentLoading?.Invoke(this, new ContentLoadingEventArgs() { ContentFileName = name, ContentType = ContentType.Texture, PackageName = this.PackageName });
            try
            {
                using (ZipFile pak = ZipFile.Read(PackagePath))
                {
                    MemoryStream ms = new MemoryStream();
                    ms.Position = 0;
                    pak[name + ".tex"].Extract(ms);
                    Texture tex = new Texture(Image.FromStream(ms), textureLayout);
                    return tex;
                }
            }
            catch (Exception ex)
            {
                throw new ContentLoadException("Unable to load \"" + name + "\" texture from " + this.PackageName + "! Inner exception message: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Loads <see cref="Animation"/> from package
        /// </summary>
        /// <param name="name">Name of <see cref="Animation"/></param>
        /// <returns><see cref="Animation"/></returns>
        public Animation LoadAnimation(string name)
        {
            ContentLoading?.Invoke(this, new ContentLoadingEventArgs() { ContentFileName = name, ContentType = ContentType.Animation, PackageName = this.PackageName });
            try
            {
                Texture texture = LoadTexture(name, TextureLayout.Stretch);
                using (ZipFile pak = ZipFile.Read(PackagePath))
                {
                    MemoryStream ms = new MemoryStream();
                    ms.Position = 0;
                    pak[name + ".amd"].Extract(ms);
                    byte[] raw = ms.ToArray();
                    string[] animationMetadata = Encoding.Default.GetString(raw).Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    int animFrmDuration = 0;
                    int animFrmCount = 0;
                    int frameWidth = 0;
                    foreach (var ln in animationMetadata)
                    {
                        string[] split = ln.Split('=');
                        switch (split[0].ToLower())
                        {
                            case "frameticktrigger":
                                if (!int.TryParse(split[1], out animFrmDuration))
                                    throw new ContentLoadException("Unable to load animation metadata of " + name + " from " + this.PackageName + "! Invalid metadata parameter value: \"" + split[0] + "=" + split[1] + "\" must be numerical Int32 value");
                                break;
                            case "framecount":
                                if (!int.TryParse(split[1], out animFrmCount))
                                    throw new ContentLoadException("Unable to load animation metadata of " + name + " from " + this.PackageName + "! Invalid metadata parameter value: \"" + split[0] + "=" + split[1] + "\" must be numerical Int32 value");
                                break;
                            case "framewidth":
                                if (!int.TryParse(split[1], out frameWidth))
                                    throw new ContentLoadException("Unable to load animation metadata of " + name + " from " + this.PackageName + "! Invalid metadata parameter value: \"" + split[0] + "=" + split[1] + "\" must be numerical Int32 value");
                                break;
                        }
                    }
                    Animation animation = new Animation(texture, animFrmCount, animFrmDuration, frameWidth);
                    return animation;
                }
            }
            catch (Exception ex)
            {
                throw new ContentLoadException("Unable to load \"" + name + "\" animation from " + this.PackageName + "! Inner exception message: " + ex.Message, ex);
            }
        }
        
        /// <summary>
        /// [CURRENTLY BROKEN] Loads <see cref="Font"/> from package
        /// </summary>
        /// <param name="name">Name of <see cref="Font"/></param>
        /// <param name="fontSize">Font size in pt</param>
        /// <returns><see cref="Font"/></returns>
        [Obsolete("Font loading from packages currently broken! Please create issue on GitHub if you want to help to fix this", true)]
        public Font LoadFont(string name, float fontSize)
        {
            ContentLoading?.Invoke(this, new ContentLoadingEventArgs() { ContentFileName = name, ContentType = ContentType.Font, PackageName = this.PackageName });
            try
            {
                //byte[] fontDataRaw = pak.ReadBytes(name + ".fnt");
                //FontFamily fml = FontFromBytesConverter.FontFamilyFromBytes(fontDataRaw);
                //Font font = new Font(fml, fontSize);
                //return font;
                throw new NotImplementedException("Font loading from packages currently broken! Please create issue on GitHub if you want to help to fix this");
            }
            catch (Exception ex)
            {
                throw new ContentLoadException("Unable to load \"" + name + "\" font from " + this.PackageName + "! Inner exception message: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Loads <see cref="Audio"/> from package
        /// </summary>
        /// <param name="name">Name of <see cref="Audio"/></param>
        /// <returns><see cref="Audio"/></returns>
        public Audio LoadAudio(string name)
        {
            ContentLoading?.Invoke(this, new ContentLoadingEventArgs() { ContentFileName = name, ContentType = ContentType.Audio, PackageName = this.PackageName });
            try
            {
                using (ZipFile pak = ZipFile.Read(PackagePath))
                {
                    MemoryStream ms = new MemoryStream();
                    ms.Position = 0;
                    pak[name + ".wad"].Extract(ms);
                    byte[] raw = ms.ToArray();
                    Audio audio = new Audio(WaveFileReaderFromBytesConverter.ByteArrayToWaveFileReader(raw));
                    return audio;
                }
            }
            catch (Exception ex)
            {
                throw new ContentLoadException("Unable to load \"" + name + "\" audio from " + this.PackageName + "! Inner exception message: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Loads <see cref="String"/> array of lines from package
        /// </summary>
        /// <param name="name">Name of strings file</param>
        /// <returns><see cref="string"/> array</returns>
        public string[] LoadStrings(string name)
        {
            ContentLoading?.Invoke(this, new ContentLoadingEventArgs() { ContentFileName = name, ContentType = ContentType.Strings, PackageName = this.PackageName });
            try
            {
                using (ZipFile pak = ZipFile.Read(PackagePath))
                {
                    MemoryStream ms = new MemoryStream();
                    ms.Position = 0;
                    pak[name + ".strings"].Extract(ms);
                    byte[] raw = ms.ToArray();
                    string[] outputStrings = Encoding.Default.GetString(raw).Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    return outputStrings;
                }
            }
            catch (Exception ex)
            {
                throw new ContentLoadException("Unable to load \"" + name + "\" strings from " + this.PackageName + "! Inner exception message: " + ex.Message, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ContentStorageCreatedEventDelegate(object sender, EventArgs e);
        /// <summary>
        /// Calls at <see cref="ContentStorage"/> was created
        /// </summary>
        public event ContentStorageCreatedEventDelegate ContentStorageCreated;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ContentLoadingEventDelegate(object sender, ContentLoadingEventArgs e);
        /// <summary>
        /// Calls at loading any content from this <see cref="ContentStorage"/>
        /// </summary>
        public event ContentLoadingEventDelegate ContentLoading;
    }

    /// <summary>
    /// <see cref="ContentStorage.ContentLoading"/> event arguments
    /// </summary>
    public sealed class ContentLoadingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets loading content filename without extention
        /// </summary>
        public string ContentFileName { get; internal set; }
        /// <summary>
        /// Gets current package name
        /// </summary>
        public string PackageName { get; internal set; }
        /// <summary>
        /// Gets loading content file type
        /// </summary>
        public ContentType ContentType { get; internal set; }
    }

    /// <summary>
    /// Types of loadable content
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// Texture content type
        /// </summary>
        Texture,
        /// <summary>
        /// Animation metadata content type
        /// </summary>
        Animation,
        /// <summary>
        /// Font content type
        /// </summary>
        Font,
        /// <summary>
        /// Wave audio content type
        /// </summary>
        Audio,
        /// <summary>
        /// Strings content type
        /// </summary>
        Strings
    }
}
