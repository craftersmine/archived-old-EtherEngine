using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.EtherEngine.Core
{
    public sealed class GameLayer
    {
        internal List<GameObject> GameObjects = new List<GameObject>();

        public void AddGameObject(GameObject gameObject)
        {
            GameObjects.Add(gameObject);
            gameObject.OnCreate();
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            if (GameObjects.Contains(gameObject))
                GameObjects.Remove(gameObject);
            gameObject.OnDestroy();
        }
    }
}
