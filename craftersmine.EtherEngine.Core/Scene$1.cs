using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.EtherEngine.Core
{
    public partial class Scene
    {
        internal List<GameObject> GameObjects { get; set; } = new List<GameObject>();
        internal List<Coroutine> Coroutines { get; set; } = new List<Coroutine>();

        public Viewport Viewport { get; set; }

        public Scene()
        {

        }

        public void AddGameObject(int index, GameObject gameObject)
        {
            GameObjects.Insert(index, gameObject);
        }

        public void AddGameObject(GameObject gameObject)
        {
            GameObjects.Add(gameObject);
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            GameObjects.Remove(gameObject);
        }

        public void RegisterCoroutine(Coroutine coroutine)
        {
            Coroutines.Add(coroutine);
        }

        public void UnregisterCoroutine(Coroutine coroutine)
        {
            Coroutines.Remove(coroutine);
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
            OnUpdate();
        }

        internal void UpdateViewport()
        {
            Viewport = new Viewport(new Transform(GameApplication.GameWnd.ClientRectangle));
        }
    }
}
