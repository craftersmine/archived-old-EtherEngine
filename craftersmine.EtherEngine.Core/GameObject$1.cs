using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using craftersmine.EtherEngine.Content;

namespace craftersmine.EtherEngine.Core
{
    public partial class GameObject
    {
        public bool IsAnimated { get; private set; }
        public Texture Texture { get; set; }
        public Animation Animation { get; set; }
        public Transform Transform { get; private set; }
        public CollisionBox CollisionBox { get; private set; }
        public string InternalGameName { get; set; }

        internal Rectangle GameObjectBase { get; private set; }
        internal Label DebugLabel { get; set; }
        internal Texture CurrentTexture { get; set; }
        internal bool IsRendererProcessed { get; set; }
        internal int InternalRandomId { get; private set; }

        public GameObject()
        { }

        public void PlayAnimation()
        {
            if (Animation != null)
                IsAnimated = true;
        }

        public void StopAnimation()
        {
            IsAnimated = false;
            CurrentTexture = Texture;
        }

        internal void OnUpdateInternal()
        {
            GameObjectBase.Width = Transform.Bounds.Width;
            GameObjectBase.Height = Transform.Bounds.Height;
            //GameObjectBase.Margin = new System.Windows.Thickness(Transform.Position.X, GameObjectBase.Margin.Top, GameObjectBase.Margin.Right, Transform.Position.Y);
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
                CurrentTexture = Animation.GetFrame(Animation.CurrentFrame);
            }
            OnUpdate();
        }

        internal void OnCreatedInternal()
        {
            InternalRandomId = new Random().Next(int.MaxValue);
            InternalGameName = "GameObject@" + InternalRandomId;
            Transform = new Transform(new Rect(0, 0, 64, 64));
            CollisionBox = new CollisionBox();
            CollisionBox.SetCollisionBox(0, 0, Transform.Bounds.Width, Transform.Bounds.Height);
            Texture = GameApplication.InternalTextures["missingtexture"];
            CurrentTexture = Texture;
            GameObjectBase = new Rectangle();
            GameObjectBase.Width = Transform.Bounds.Width;
            GameObjectBase.Height = Transform.Bounds.Height;
            GameObjectBase.Margin = new Thickness(Transform.RelativeCameraPosition.X, GameObjectBase.Margin.Top, GameObjectBase.Margin.Right, Transform.RelativeCameraPosition.Y);
            OnCreated();
        }
    }
}