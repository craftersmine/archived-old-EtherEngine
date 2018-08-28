using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.EtherEngine.Core
{
    public struct WindowSize
    {
        public int Width { get; }
        public int Height { get; }

        public WindowSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public WindowSize(WindowSize preset)
        {
            Width = preset.Width;
            Height = preset.Height;
        }

        public Size GetSize()
        {
            return new Size(Width, Height);
        }

        public override string ToString()
        {
            return Width + "x" + Height;
        }
    }

    public static class WindowSizePresets
    {
        /// <summary>
        /// FullHD size (1920x1080) (16:9)
        /// </summary>
        public static WindowSize FullHD { get; } = new WindowSize(1920, 1080);
        /// <summary>
        /// HD size (1280x720) (16:9)
        /// </summary>
        public static WindowSize HD { get; } = new WindowSize(1280, 720);
        /// <summary>
        /// HD+ size (1600x900) (16:9)
        /// </summary>
        public static WindowSize HDPlus { get; } = new WindowSize(1600, 900);
        /// <summary>
        /// WQHD size or 2k size (2560x1440) (16:9)
        /// </summary>
        [Obsolete("This resolution is too large and because it not recommended", false)]
        public static WindowSize WQHD { get; } = new WindowSize(2560, 1440);
        /// <summary>
        /// UHD size or 4k size (3840x2160) (16:9)
        /// </summary>
        [Obsolete("This resolution is too large and because it not recommended", false)]
        public static WindowSize UHD { get; } = new WindowSize(3840, 2160);
        /// <summary>
        /// SVGA size (800x600) (4:3)
        /// </summary>
        [Obsolete("This resolution is too old and because it not recommended", false)]
        public static WindowSize SVGA { get; } = new WindowSize(800, 600);
        /// <summary>
        /// XGA size (1024x768) (4:3)
        /// </summary>
        public static WindowSize XGA { get; } = new WindowSize(1024, 768);
        /// <summary>
        /// WXGA size (1280x768) (5:3)
        /// </summary>
        public static WindowSize WXGA { get; } = new WindowSize(1280, 768);
        /// <summary>
        /// SXGA size (1280x1024) (5:4)
        /// </summary>
        [Obsolete("This resolution is too old and because it not recommended", false)]
        public static WindowSize SXGA { get; } = new WindowSize(1280, 1024);
    }
}
