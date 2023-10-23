using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using GameEngine.ComponentManagement;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    [Serializable]
    public class Scene : ICloneable, IDisposable
    {
        public event Action<GameObject> OnNewObjectLoaded;
        public event Action<GameObject> OnObjectUnloaded;

        public bool ActiveScene;

        public string name;

        //List<GameObject> gameObjects = new List<GameObject>();
        //public ComponentCollection components = new ComponentCollection();
        GameObjectCollection gameObjects = new GameObjectCollection();

        public GameObject[] GetGameobjects() => gameObjects.ToArray();
        public GameObjectCollection GetGameObjectCollection() => gameObjects;

        public void LoadGameobject(GameObject gameObject)
        {
            gameObjects.AddGameObject(gameObject);
            OnNewObjectLoaded?.Invoke(gameObject);
        }

        public void UnloadGameobject(GameObject gameObject) 
        {
            gameObjects.RemoveGameObject(gameObject);
            OnObjectUnloaded?.Invoke(gameObject);
        }

        public object Clone()
        {
            Scene clone = new Scene();
            clone.name = name + "Clone";

            clone.gameObjects = gameObjects.Clone() as GameObjectCollection;

            return clone;
        }

        public void Dispose() 
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if(disposing) 
            {
               gameObjects.Clear(); 
            }
        }
    }
}
