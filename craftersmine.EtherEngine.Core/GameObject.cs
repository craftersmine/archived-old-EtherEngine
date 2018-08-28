using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using craftersmine.EtherEngine.Content;
using craftersmine.EtherEngine.Utils;

namespace craftersmine.EtherEngine.Core
{
    public partial class GameObject
    {
        public int X { get { return Transform.X; } }
        public int Y { get { return Transform.Y; } }
        public int Width { get { return Transform.Width; } }
        public int Height { get { return Transform.Height; } }
        public int CameraOffsetX { get; internal set; }
        public int CameraOffsetY { get; internal set; }
        public Transform Transform { get; internal set; } = new Transform(0, 0, 0, 0);
        public bool IsIlluminatingLight { get; set; }
        public Color LightColor { get; set; }
        public Texture LightCookieTexture { get; set; }
        public CollisionBox CollisionBox { get; internal set; } = new CollisionBox();
        public Animation Animation { get; set; }
        public bool IsAnimated { get; private set; }
        public bool IsCameraVisible { get; set; }

        internal Texture ObjectTexture { get; set; }
        internal Image DrawableImage {
            get {
                if (!IsAnimated)
                {
                    if (ObjectTexture.TextureLayout != TextureLayout.Tile)
                        return ObjectTexture.TextureImage;
                    else return CachedTiledTexture.TextureImage;
                }
                else return AnimationFrameImage;
            }
        }
        internal Texture CachedTiledTexture { get; set; }
        internal bool IsInvalidTiledTextureCache { get; set; } = true;

        private Image AnimationFrameImage { get; set; }

        public void SetTexture(Texture texture)
        {
            ObjectTexture = texture;
        }

        public void Animate()
        {
            if (Animation != null)
                IsAnimated = true;
        }

        public virtual void OnCreate()
        {

        }

        public virtual void OnPhysicsUpdate()
        {

        }

        public virtual void OnCollision(GameObject gameObject)
        {

        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnDestroy()
        {

        }

        public virtual void OnMouseHover(int xMousePos, int yMousePos)
        {

        }

        public virtual void OnMouseClick(MouseButtons mouseButtons, int xMousePos, int yMousePos)
        {

        }

        internal void InternalUpdate()
        {
            if (IsAnimated)
            {
                Animation.CountedTicks++;
                if (Animation.CountedTicks == Animation.FrameTickTrigger)
                {
                    Animation.CountedTicks = 0;
                    if (Animation.CurrentFrame == Animation.AnimationFramesCount - 1)
                        Animation.CurrentFrame = 0;
                    else Animation.CurrentFrame++;
                }
                AnimationFrameImage = Animation.GetFrame(Animation.CurrentFrame);
            }
        }

        internal void InternalPhysicsUpdate()
        {
            CollisionBox.UpdateCollisionBox(this.Transform);
        }

        internal void CheckCollisionFor(List<GameObject> objects)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] != this)
                {
                    if (this.CollisionBox.CollisionBoxBoundings.IntersectsWith(objects[i].CollisionBox.CollisionBoxBoundings))
                    {
                        this.OnCollision(objects[i]);
                    }
                }
            }
        }
    }
}
