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
    public sealed class Renderer
    { 
        private Thread DrawThread { get; set; }
        private bool IsRunning { get { return GameApplication.IsProcessActive; } }
        private Bitmap DrawBuffer { get; set; }
        private Bitmap LightingBuffer { get; set; }
        private Graphics MainBufferDrawer { get; set; }
        private Graphics LightBufferDrawer { get; set; }
        private int DrawCall { get; set; }
        private int LastDrawCall { get; set; }
        private int Framerate { get; set; }
        private System.Threading.Timer FPSTimer { get; set; }

        private Pen DebugRectangleBoundings { get; set; } = new Pen(Color.Yellow, 1);
        private Pen DebugCameraRectangleBoundings { get; set; } = new Pen(Color.Blue, 1);
        private Pen DebugCollisionBoxRectangleBoundings { get; set; } = new Pen(Color.Red, 1);
        private Pen DebugMousePointRectangleBoundings { get; set; } = new Pen(Color.Lime, 1);
        private SolidBrush FontBrush { get; set; } = new SolidBrush(Color.Yellow);

        public InterpolationMode TextureInterpolationMode { get; set; }
        public bool DrawDebugInfo { get; set; }
        public bool DrawDebugRects { get; set; }
        public string AdditionalDebugInfo { get; set; }

        private void DrawCaller()
        {
            lock (GameApplication.GameWindow.Canvas.RazorLock)
            {
                while (IsRunning)
                {
                    // Clear light buffer
                    //LightBufferDrawer.Clear(Color.Black);
                    // Set interpolation mode
                    MainBufferDrawer.InterpolationMode = TextureInterpolationMode;

                    if (GameApplication.GameWindow.CurrentScene != null)
                    {
                        // Clear Main buffer with scene color
                        MainBufferDrawer.Clear(GameApplication.GameWindow.CurrentScene.BackgroundColor);
                        // Draw background
                        _drawBg();
                        _drawGameObjects();
                        //_calcLighting();
                        _drawUiLayer();
                    }
                    else
                    {
                        // Or clear all with base color
                        MainBufferDrawer.Clear(GameApplication.GameWindow.BaseColor);
                    }
                    // Render
                    GameApplication.GameWindow.Canvas.RazorGFX.DrawImage(DrawBuffer, 0, 0);
                    if (DrawDebugRects)
                    {
                        GameApplication.GameWindow.Canvas.RazorGFX.DrawRectangle(DebugCameraRectangleBoundings, GameApplication.GameWindow.CurrentScene.Camera.CameraBounds);
                        GameApplication.GameWindow.Canvas.RazorGFX.DrawRectangle(DebugMousePointRectangleBoundings, GameApplication.GameWindow.MousePoint);
                    }
                    if (DrawDebugInfo)
                        GameApplication.GameWindow.Canvas.RazorGFX.DrawString($"craftersmine EtherEngine{Environment.NewLine}{Environment.NewLine}WindowSize: {GameApplication.GameWindow.Width}x{GameApplication.GameWindow.Height}{Environment.NewLine}DrawCall: {DrawCall} {Environment.NewLine}FPS: {Framerate}{Environment.NewLine}{AdditionalDebugInfo}", GameApplication.GameWindow.Font, FontBrush, 0, 0);
                    GameApplication.GameWindow.Canvas.RazorPaint();
                    DrawCall++;
                }
            }
        }

        private void _drawUiLayer()
        {
            if (GameApplication.GameWindow.CurrentScene.UIControls.Any())
            {
                for (int uiInd = 0; uiInd < GameApplication.GameWindow.CurrentScene.UIControls.Count; uiInd++)
                {
                    GameApplication.GameWindow.CurrentScene.UIControls[uiInd].CallDrawMethod();
                    if (GameApplication.GameWindow.CurrentScene.UIControls[uiInd].DrawableImage != null)
                    {
                        MainBufferDrawer.DrawImage(GameApplication.GameWindow.CurrentScene.UIControls[uiInd].DrawableImage, GameApplication.GameWindow.CurrentScene.UIControls[uiInd].Transform.X, GameApplication.GameWindow.CurrentScene.UIControls[uiInd].Transform.Y);
                    }
                }
            }
        }

        private void _calcLighting()
        {
            SetImageOpacity(LightingBuffer, 1.0f - GameApplication.GameWindow.CurrentScene.LightValue);
            MainBufferDrawer.DrawImage(LightingBuffer, 0, 0);
        }

        private void _drawBg()
        {
            if (GameApplication.GameWindow.CurrentScene.BackgroundTexture != null)
            {
                MainBufferDrawer.DrawImage(GameApplication.GameWindow.CurrentScene.BackgroundTexture.TextureImage, 0, 0);
            }
        }

        private void _drawGameObjects()
        {
            if (GameApplication.GameWindow.CurrentScene.GameObjects.Any())
            {
                for (int gObjIndex = 0; gObjIndex < GameApplication.GameWindow.CurrentScene.GameObjects.Count; gObjIndex++)
                {
                    if (GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Transform.CameraBoundings.IntersectsWith(GameApplication.GameWindow.CurrentScene.Camera.CameraBounds))
                    {
                        GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].IsCameraVisible = true;
                        if (GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].ObjectTexture.TextureLayout == TextureLayout.Tile && GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].IsInvalidTiledTextureCache)
                        {
                            GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].CachedTiledTexture = PrepareTiledTexture(GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].ObjectTexture, GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Transform.BoundingsRectangle);
                            GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].IsInvalidTiledTextureCache = false;
                        }
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
                        if (DrawDebugRects)
                        {
                            MainBufferDrawer.DrawRectangle(DebugRectangleBoundings, GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Transform.CameraBoundings);
                            MainBufferDrawer.DrawRectangle(DebugCollisionBoxRectangleBoundings, GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].CollisionBox.CollisionBoxBoundings);
                        }
                        if (DrawDebugInfo)
                            MainBufferDrawer.DrawString("X: " + GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].X + "\r\nY: " + GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Y + "\r\nCamX: " + GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Transform.CameraX + "\r\nCamY: " + GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Transform.CameraY, GameApplication.GameWindow.Font, FontBrush, GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Transform.CameraX, GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].Transform.CameraY);
                    }
                    else GameApplication.GameWindow.CurrentScene.GameObjects[gObjIndex].IsCameraVisible = false;
                }
            }
        }

        private Bitmap SetImageOpacity(Bitmap image, float opacity)
        {
            //create a graphics object from the image  
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

        private void FPSCounter(object state)
        {
            Framerate = DrawCall - LastDrawCall;
            LastDrawCall = DrawCall;
        }

        private Texture PrepareTiledTexture(Texture texture, Rectangle textureBounds)
        {
            int xCount = textureBounds.Width / texture.TextureImage.Width + 1;
            int yCount = textureBounds.Height / texture.TextureImage.Height + 1;
            Bitmap tiledTexture = new Bitmap(textureBounds.Width, textureBounds.Height);
            Graphics painter = Graphics.FromImage(tiledTexture);
            painter.InterpolationMode = this.TextureInterpolationMode;
            using (TextureBrush tiledBrush = new TextureBrush(texture.TextureImage))
                painter.FillRectangle(tiledBrush, new Rectangle(new Point(0, 0), textureBounds.Size));
            return new Texture(tiledTexture, TextureLayout.Tile);
        }

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
