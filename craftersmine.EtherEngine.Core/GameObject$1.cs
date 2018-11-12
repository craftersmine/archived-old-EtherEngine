using System;
using System.Collections.Generic;
using System.Windows.Controls;
using craftersmine.EtherEngine.Content;

namespace craftersmine.EtherEngine.Core
{
    public partial class GameObject
    {
        public bool IsAnimated { get; private set; }
        public Texture Texture { get; set; }
        public Animation Animation { get; set; }
        public Transform Transform { get; private set; }

        internal Image GameObjectBase { get; private set; }
        internal Texture CurrentTexture { get; set; }

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
            Transform = new Transform(new System.Drawing.Rectangle(0, 0, 64, 64));
            GameObjectBase = new Image();
            GameObjectBase.Width = Transform.Bounds.Width;
            GameObjectBase.Height = Transform.Bounds.Height;
            OnCreated();
        }
    }
}