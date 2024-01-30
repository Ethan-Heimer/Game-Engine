using GameEngine.Engine.Events;
using Microsoft.Build.Tasks.Xaml;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine
{
    [ContainsEvents]
    public static class GameObjectManager
    {
        static EngineEvent<GameObjectsAddedEvent> OnObjectsAdded;
        static EngineEvent<GameObjectsRemovedEvent> OnObjectsRemoved;

        static EngineEvent<GameObjectTreeChanged> OnTreeChanged;
       
        static List<GameObject> gameObjects = new List<GameObject>();

        public static GameObject[] GetGameObjects() => gameObjects.ToArray();

        public static void RegisterGameobject(GameObject gameObject) 
        {
            gameObjects.Add(gameObject);
            OnObjectsAdded?.Invoke(new GameObjectsAddedEvent()
            {
                AddedGameObjects = new GameObject[] { gameObject },
                TotalGameObjects = gameObjects.ToArray()
            });
            Console.WriteLine("Gameobject Added");

            AlertTreeChange();
        }

        public static void RegisterGameobjectGroup(GameObject[] _gameObjects)
        {
            gameObjects.AddRange(_gameObjects);

            OnObjectsAdded?.Invoke(new GameObjectsAddedEvent()
            {
                AddedGameObjects = _gameObjects,
                TotalGameObjects = gameObjects.ToArray()
            });
            Console.WriteLine("Gameobject Added");

            AlertTreeChange();
        }

        public static void DispatchGameobject(GameObject gameObject) 
        {
            gameObjects.Remove(gameObject);

            gameObject.ClearComponents();

            OnObjectsRemoved?.Invoke(new GameObjectsRemovedEvent()
            {
                RemovedGameObjects = new GameObject[] { gameObject },
                TotalGameObjects = gameObjects.ToArray()
            });

            Console.WriteLine("Removed");

            AlertTreeChange();
        }

        public static void DispatchGameobjectGroup(GameObject[] _gameObjects)
        {
            foreach(GameObject o in _gameObjects)
                gameObjects.Remove(o);

            OnObjectsRemoved?.Invoke(new GameObjectsRemovedEvent()
            {
                RemovedGameObjects = _gameObjects,
                TotalGameObjects = gameObjects.ToArray()
            });
            Console.WriteLine("Gameobject Added");

            AlertTreeChange();
        }

        public static void ClearAll()
        {
            while(gameObjects.Count > 0) 
            {
                GameObject gameObject = gameObjects[0];
                gameObject.Destroy();
            }

            AlertTreeChange();
        }

        public static void AlertTreeChange()
        {
            OnTreeChanged?.Invoke(new GameObjectTreeChanged()
            {
                TotalGameObjects = gameObjects.ToArray(),
            });
        }

        public static GameObject[] GetOverlapping(Rectangle rectangle)
        {
            //O(N)
            return gameObjects.Where(x => x.Transform.Bounds.Intersects(rectangle)).ToArray();
        }

    }

    public class GameObjectEvent : IEventArgs
    {
        public GameObject[] TotalGameObjects
        {
            get; set;
        }
        public object Sender
        {
            get; set;
        }
    }

    public class GameObjectsAddedEvent : GameObjectEvent 
    {
        public GameObject[] AddedGameObjects;
    }
    public class GameObjectsRemovedEvent : GameObjectEvent 
    {
        public GameObject[] RemovedGameObjects;
    }

    public class GameObjectTreeChanged : GameObjectEvent
    {
        public GameObject ChangedObject;
    }
}
