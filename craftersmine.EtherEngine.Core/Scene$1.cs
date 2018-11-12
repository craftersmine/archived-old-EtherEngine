using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace craftersmine.EtherEngine.Core
{
    public partial class Scene
    {
        internal List<GameObject> GameObjects { get; set; } = new List<GameObject>();

        public Viewport Viewport { get; set; }
        public Color BackgroundColor { get; set; } = Colors.Black;

        public Scene()
        {

        }

        public void AddGameObject(int index, GameObject gameObject)
        {
            GameObjects.Insert(index, gameObject);
            gameObject.OnCreatedInternal();
        }

        public void AddGameObject(GameObject gameObject)
        {
            GameObjects.Add(gameObject);
            gameObject.OnCreatedInternal();
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            GameObjects.Remove(gameObject);
        }

        internal void OnSceneShownInternal()
        {
            UpdateViewport();
            OnShown();
        }

        internal void OnSceneDestroyedInternal()
        {
            OnDestroyed();
        }

        internal void OnSceneUpdateInternal()
        {
            for (int gObj = 0; gObj < GameObjects.Count; gObj++)
                GameObjects[gObj].Transform.UpdateCoordsRelativeViewport(Viewport.Transform);
            OnUpdate();
        }

        internal void UpdateViewport()
        {
            Viewport = new Viewport(new Transform(new Rect(GameApplication.GameWnd.ClientRectangle.X, GameApplication.GameWnd.ClientRectangle.Y, GameApplication.GameWnd.ClientRectangle.Width, GameApplication.GameWnd.ClientRectangle.Height)));
        }
    }
}
