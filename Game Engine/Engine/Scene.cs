using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using GameEngine.ComponentManagement;
using GameEngine.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    [Serializable]
    public class Scene : IDisposable
    {
        public string name;

        public GameObject[] gameObjects = new GameObject[0];
        
        public GameObject[] GetGameobjects() => gameObjects.ToArray();

        public void Save()
        {
            gameObjects = GameObjectManager.GetGameObjects();
        }

        public void Load()
        {
            if(gameObjects != null)
                GameObjectManager.RegisterGameobjectGroup(gameObjects);
        }

        public void Unload()
        {
            Console.WriteLine("Unload");
            if (gameObjects != null)
                GameObjectManager.ClearAll();
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
                gameObjects = new GameObject[0];
            }
        }
    }
}
