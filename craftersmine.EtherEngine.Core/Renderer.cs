using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using craftersmine.EtherEngine.Content;

namespace craftersmine.EtherEngine.Core
{
    /// <summary>
    /// Represents main game renderer. This class cannot be inherited
    /// </summary>
    public sealed class Renderer
    { 
        private Thread DrawThread { get; set; }   // Draw thread
        private bool IsRunning { get { return GameApplication.IsProcessActive; } }  // Is Renderer working
        private Bitmap DrawBuffer { get; set; }  // Main graphics draw buffer
        private Bitmap LightingBuffer { get; set; }  // Lighting graphics draw buffer
        private Graphics MainBufferDrawer { get; set; }  // Main graphics drawer
        private Graphics LightBufferDrawer { get; set; }  // Light graphics drawer
        private int DrawCall { get; set; }  // Counted drawcalls
        private int LastDrawCall { get; set; }  // Last FPS counter written draw call
        private System.Threading.Timer FPSTimer { get; set; }  // FPS counter

        // GameObject boundings rectangle pen
        private Pen DebugRectangleBoundings { get; set; } = new Pen(Color.Yellow, 1);
        // Camera boundings rectangle pen
        private Pen DebugCameraRectangleBoundings { get; set; } = new Pen(Color.Blue, 1);
        // GameObject Collision box rectangle pen
        private Pen DebugCollisionBoxRectangleBoundings { get; set; } = new Pen(Color.Red, 1);
        // Mouse point pen
        private Pen DebugMousePointRectangleBoundings { get; set; } = new Pen(Color.Lime, 1);
        // Debug info font brush
        private SolidBrush FontBrush { get; set; } = new SolidBrush(Color.Yellow);

        /// <summary>
        /// Gets or sets renderer texture interpolation mode
        /// </summary>
        public InterpolationMode TextureInterpolationMode { get; set; }
        /// <summary>
        /// Gets or sets true if debug info is draws else false
        /// </summary>
        public bool DrawDebugInfo { get; set; }
        /// <summary>
        /// Gets or sets true if debug rectangles is draws else false
        /// </summary>
        public bool DrawDebugRects { get; set; }
        /// <summary>
        /// Gets or sets additional debug info at main debug info on screen
        /// </summary>
        public string AdditionalDebugInfo { get; set; }

        public int Framerate { get; internal set; } 

        // Main draw caller
        private void DrawCaller()
        {
            lock (GameApplication.GameWindow.Canvas.RazorLock)  // Set lock for resize or etc.
            {
                while (IsRunning)
                {
                    // Clear light buffer
                    //LightBufferDrawer.Clear(Color.Black);
                    // Set interpolation mode
                    MainBufferDrawer.InterpolationMode = TextureInterpolationMode;
                    // If Current Scene not null
                    if (GameApplication.GameWindow.CurrentScene != null)
                    {
                        // Clear Main buffer with scene color
                        MainBufferDrawer.Clear(GameApplication.GameWindow.CurrentScene.BackgroundColor);
                        // Draw background
                        _drawBg();
                        // Draw gameobjects
                        _drawGameObjects();
                        // Calculate and draw lights
                        //_calcLighting();
                        // Draw UI controls
                        _drawUiLayer();
                    }
                    else
                    {
                        // Or if Current Scene is null just clear all window with base color
                        MainBufferDrawer.Clear(GameApplication.GameWindow.BaseColor);
                    }
                    // Draw main buffer
                    GameApplication.GameWindow.Canvas.RazorGFX.DrawImage(DrawBuffer, 0, 0);
                    if (DrawDebugRects)  // Draw debug rects if true
                    {
                        GameApplication.GameWindow.Canvas.RazorGFX.DrawRectangle(DebugCameraRectangleBoundings, GameApplication.GameWindow.CurrentScene.Camera.CameraBounds);
                        GameApplication.GameWindow.Canvas.RazorGFX.DrawRectangle(DebugMousePointRectangleBoundings, GameApplication.GameWindow.MousePoint);
                    }
                    if (DrawDebugInfo)  // Draw debug texts if true
                        GameApplication.GameWindow.Canvas.RazorGFX.DrawString($"craftersmine EtherEngine{Environment.NewLine}{Environment.NewLine}WindowSize: {GameApplication.GameWindow.Width}x{GameApplication.GameWindow.Height}{Environment.NewLine}DrawCall: {DrawCall} {Environment.NewLine}FPS: {Framerate}{Environment.NewLine}{AdditionalDebugInfo}", GameApplication.GameWindow.Font, FontBrush, 0, 0);
                    // Render
                    GameApplication.GameWindow.Canvas.RazorPaint();
                    DrawCall++;  // Increment draw call
                }
            }
        }

        // UI layer drawer
        private void _drawUiLayer()
        {
            // If current scene have any UIControl, start draw
            if (GameApplication.GameWindow.CurrentScene.UIControls.Any())
            {
                for (int uiInd = 0; uiInd < GameApplication.GameWindow.CurrentScene.UIControls.Count; uiInd++)
                {
                    GameApplication.GameWindow.CurrentScene.UIControls[uiInd].CallDrawMethod();  // Call user-defined draw method
                    if (GameApplication.GameWindow.CurrentScene.UIControls[uiInd].DrawableImage != null)  // If drawable image not null, drae it
                    {
                        MainBufferDrawer.DrawImage(GameApplication.GameWindow.CurrentScene.UIControls[uiInd].DrawableImage, GameApplication.GameWindow.CurrentScene.UIControls[uiInd].Transform.X, GameApplication.GameWindow.CurrentScene.UIControls[uiInd].Transform.Y);
                    }
                }
            }
        }

        // Global illumination and objects illumination (Lighting)
        private void _calcLighting()
        {
            // Set light buffer opacity and draw it on main buffer
            SetImageOpacity(LightingBuffer, 1.0f - GameApplication.GameWindow.CurrentScene.LightValue);
            MainBufferDrawer.DrawImage(LightingBuffer, 0, 0);
        }

        // Just draws background
        private void _drawBg()
        {
            if (GameApplication.GameWindow.CurrentScene.BackgroundTexture != null)
            {
                MainBufferDrawer.DrawImage(GameApplication.GameWindow.CurrentScene.BackgroundTexture.TextureImage, 0, 0);
            }
        }

        // Game Object drawer
        private void _drawGameObjects()
        {
            // If current scene have any gameobject draw it
            if (GameApplication.GameWindow.CurrentScene.GameObjects.Any())
            {
                for (int gObjIndex = 0; gObjIndex < GameApplication.GameWindow.CurrentScene.GameObjects.Count; gObjIndex++)
                {
                    // If game object bounding box intersects with camera bounding box (In viewport) continue
                    if (GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Transform.CameraBoundings.IntersectsWith(GameApplication.GameWindow.CurrentScene.Camera.CameraBounds))
                    {
                        GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].IsCameraVisible = true;  // Set that object is in viewport
                        // If game object layout is tiled and game object have invalid cached tiled texture, then
                        if (GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].ObjectTexture.TextureLayout == TextureLayout.Tile && GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].IsInvalidTiledTextureCache)
                        {
                            // Prepare and save tiled texture in cache
                            GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].CachedTiledTexture = PrepareTiledTexture(GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].ObjectTexture, GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Transform.BoundingsRectangle);
                            // Set that game object have now valid tiled texture in cache
                            GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].IsInvalidTiledTextureCache = false;
                        }
                        // Draw game object texture in main buffer
                        MainBufferDrawer.DrawImage(GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].DrawableImage, GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Transform.CameraBoundings);

                        //if (GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].IsIlluminatingLight)
                        //{
                        //    GraphicsPath lightPath = new GraphicsPath();
                        //    Rectangle lightRect = new Rectangle((GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Transform.Width * 5) / 2 - (GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Transform.Width / 2), (GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Transform.Height * 5) / 2 - (GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Transform.Height / 2), GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Transform.Width * 5, GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Transform.Height * 5);
                        //    lightPath.AddEllipse(lightRect);
                        //    using (PathGradientBrush grad = new PathGradientBrush(lightPath))
                        //    {
                        //        grad.CenterColor = GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].LightColor;
                        //        grad.SurroundColors = new Color[] { Color.Black };
                        //        LightBufferDrawer.FillRectangle(grad, lightRect);
                        //    }
                        //}

                        // If true draw debug rectangles (Main bounding box and collision box)
                        if (DrawDebugRects)
                        {
                            MainBufferDrawer.DrawRectangle(DebugRectangleBoundings, GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Transform.CameraBoundings);  // GameObject main bounding box
                            MainBufferDrawer.DrawRectangle(DebugCollisionBoxRectangleBoundings, GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].CollisionBox.CollisionBoxBoundings);  // GameObject collision bounding box
                        }
                        // Draw debug info about that object
                        if (DrawDebugInfo)
                            MainBufferDrawer.DrawString("X: " + GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].X + "\r\nY: " + GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Y + "\r\nCamX: " + GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Transform.CameraX + "\r\nCamY: " + GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Transform.CameraY, GameApplication.GameWindow.Font, FontBrush, GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Transform.CameraX, GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Transform.CameraY);
                    }
                    // Or if game object don't intersects with camera bounding box (Not in viewport), set that game object no being visible by camera
                    else GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].IsCameraVisible = false;
                }
            }
        }

        // Sets whole image or bitmap opacity level
        private Bitmap SetImageOpacity(Bitmap image, float opacity)
        {
            // create a graphics object from the image  
            using (Graphics gfx = Graphics.FromImage(image))
            {

                //create a color matrix object  
                ColorMatrix matrix = new ColorMatrix();

                //set the opacity  
                matrix.Matrix33 = opacity;

                //create image attributes  
                ImageAttributes attributes = new ImageAttributes();

                //set the color(opacity) of the image  
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                //now draw the image  
                gfx.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            }
            return image;
        }

        /// <summary>
        /// Runs renderer threads
        /// </summary>
        public void Run()
        {
            FPSTimer = new System.Threading.Timer(new TimerCallback(FPSCounter), null, 0, 1000);
            DrawBuffer = new Bitmap(GameApplication.GameWindow.Width, GameApplication.GameWindow.Height);
            LightingBuffer = new Bitmap(GameApplication.GameWindow.Width, GameApplication.GameWindow.Height);
            MainBufferDrawer = Graphics.FromImage(DrawBuffer);
            LightBufferDrawer = Graphics.FromImage(LightingBuffer);
            DrawThread = new Thread(new ThreadStart(DrawCaller));
            DrawThread.Start();
        }

        // FPS counter thread method body
        private void FPSCounter(object state)
        {
            Framerate = DrawCall - LastDrawCall;
            LastDrawCall = DrawCall;
        }

        // Prepares tiled texture
        private Texture PrepareTiledTexture(Texture texture, Rectangle textureBounds)
        {
            // Set count of tiles + 1 to fit in bounding box
            int xCount = textureBounds.Width / texture.TextureImage.Width + 1;
            int yCount = textureBounds.Height / texture.TextureImage.Height + 1;
            // Create buffer of tiled texture and painter
            Bitmap tiledTexture = new Bitmap(textureBounds.Width, textureBounds.Height);
            Graphics painter = Graphics.FromImage(tiledTexture);
            // Set tiled painter interpolation mode from global renderer
            painter.InterpolationMode = this.TextureInterpolationMode;
            // Fill whole tiled texture buffer with texture
            using (TextureBrush tiledBrush = new TextureBrush(texture.TextureImage))
                painter.FillRectangle(tiledBrush, new Rectangle(new Point(0, 0), textureBounds.Size));
            // Return texture
            return new Texture(tiledTexture, TextureLayout.Tile);
        }

        // Draw texture at specific position and size (Currently unused)
        private void DrawTexture(Texture texture, int xPos, int yPos, int width, int height)
        {
            Rectangle textureBounding = new Rectangle(xPos, yPos, width, height);
            switch (texture.TextureLayout)
            {
                case TextureLayout.Stretch:
                    MainBufferDrawer.DrawImage(texture.TextureImage, textureBounding);
                    break;
                case TextureLayout.Tile:
                    Texture tiledTex = PrepareTiledTexture(texture, textureBounding);
                    MainBufferDrawer.DrawImage(tiledTex.TextureImage, textureBounding);
                    break;
                case TextureLayout.Center:
                    int xCenter = (width / 2) - (texture.TextureImage.Width / 2);
                    int yCenter = (height / 2) - (texture.TextureImage.Height / 2);
                    textureBounding.X = xCenter;
                    textureBounding.Y = yCenter;
                    textureBounding.Width = texture.TextureImage.Width;
                    textureBounding.Height = texture.TextureImage.Height;
                    MainBufferDrawer.DrawImage(texture.TextureImage, textureBounding);
                    break;
            }
        }
    }
}
