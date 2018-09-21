using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using craftersmine.EtherEngine.Content;

namespace craftersmine.EtherEngine.Core
{
    /// <summary>
    /// Represents game scene
    /// </summary>
    public class Scene
    {
        internal List<GameObject> GameObjects { get; set; } = new List<GameObject>();
        internal List<UIControl> UIControls { get; set; } = new List<UIControl>();
        internal Dictionary<string, AudioChannel> AudioChannels { get; set; } = new Dictionary<string, AudioChannel>();
        /// <summary>
        /// Gets scene camera
        /// <summary>
        public Camera Camera { get; internal set; }
        /// <summary>
        /// Gets or sets scene background color
        /// </summary>
        public Color BackgroundColor { get; set; }
        /// <summary>
        /// Gets scene width (window width)
        /// </summary>
        public int Width { get { return GameApplication.GameWindowSize.Width; } }
        /// <summary>
        /// Gets scene height (window height)
        /// </summary>
        public int Height { get { return GameApplication.GameWindowSize.Height; } }
        /// <summary>
        /// Gets or sets global lighting value
        /// </summary>
        public float LightValue { get; set; }
        /// <summary>
        /// Gets or sets scene background texture
        /// </summary>
        public Texture BackgroundTexture { get; set; }

        /// <summary>
        /// Creates new instance of <see cref="Scene"/>
        /// </summary>
        public Scene()
        {
            Camera = new Camera(0, 0, Width, Height);
        }

        /// <summary>
        /// Adds <see cref="GameObject"/> to scene
        /// </summary>
        /// <param name="gameObject">Game Object to add</param>
        public void AddGameObject(GameObject gameObject)
        {
            GameObjects.Add(gameObject);
            gameObject.OnCreate();
        }
        /// <summary>
        /// Removes <see cref="GameObject"/> from scene
        /// </summary>
        /// <param name="gameObject">Game Object instance to remove</param>
        public void RemoveGameObject(GameObject gameObject)
        {
            GameObjects.Remove(gameObject);
            gameObject.OnDestroy();
        }
        /// <summary>
        /// Adds <see cref="UIControl"/> to scene
        /// </summary>
        /// <param name="control">UI control to add</param>
        public void AddUIControl(UIControl control)
        {
            UIControls.Add(control);
        }
        /// <summary>
        /// Removes <see cref="UIControl"/> from scene
        /// </summary>
        /// <param name="control">UI control instance to remove</param>
        public void RemoveUIControl(UIControl control)
        {
            UIControls.Remove(control);
        }
        /// <summary>
        /// Calls at create event
        /// </summary>
        public virtual void OnCreate()
        {

        }
        /// <summary>
        /// Calls at game update event
        /// </summary>
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
