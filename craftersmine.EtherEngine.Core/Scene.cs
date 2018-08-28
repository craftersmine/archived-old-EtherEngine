using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using craftersmine.EtherEngine.Content;

namespace craftersmine.EtherEngine.Core
{
    public class Scene
    {
        internal List<GameObject> GameObjects { get; set; } = new List<GameObject>();
        internal List<UIControl> UIControls { get; set; } = new List<UIControl>();
        internal Dictionary<string, AudioChannel> AudioChannels { get; set; } = new Dictionary<string, AudioChannel>();
        public Camera Camera { get; internal set; }

        public Color BackgroundColor { get; set; }
        
        public int Width { get { return GameApplication.GameWindowSize.Width; } }
        public int Height { get { return GameApplication.GameWindowSize.Height; } }

        public float LightValue { get; set; }

        public Texture BackgroundTexture { get; set; }

        public Scene()
        {
            Camera = new Camera(0, 0, Width, Height);
        }

        public void AddGameObject(GameObject gameObject)
        {
            GameObjects.Add(gameObject);
            gameObject.OnCreate();
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            GameObjects.Remove(gameObject);
            gameObject.OnDestroy();
        }

        public void AddUIControl(UIControl control)
        {
            UIControls.Add(control);
        }

        public void RemoveUIControl(UIControl control)
        {
            UIControls.Remove(control);
        }

        public virtual void OnCreate()
        {

        }

        public virtual void OnUpdate()
        {

        }

        internal void UpdateCameraOffsettedPositions()
        {
            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Transform.SetCameraPosition(Camera.X, Camera.Y);
            }
        }
    }
}
