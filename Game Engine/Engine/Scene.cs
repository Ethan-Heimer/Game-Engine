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
using GameEngine.Engine.Physics;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    [Serializable]
    public class Scene : IDisposable
    {
        public string name;

        public GameObject[] gameObjects = new GameObject[0];

        [NonSerialized]
        PhysicsWorld physicsWorld = new PhysicsWorld();
        public PhysicsWorld PhysicsWorld
        {
            get
            {
                if(physicsWorld == null)
                    physicsWorld = new PhysicsWorld();

                return physicsWorld;
            }
        }
        
        public GameObject[] GetGameobjects() => gameObjects.ToArray();

        public void Save()
        {
            gameObjects = GameObjectManager.GetGameObjects();
        }

        public void Load()
        {
            Console.WriteLine(gameObjects.Length + " length");
            GameObjectManager.RegisterGameobjectGroup(gameObjects);
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
