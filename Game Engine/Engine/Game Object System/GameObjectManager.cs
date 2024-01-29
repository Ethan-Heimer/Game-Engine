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
        static EngineEvent<GameObjectAddedEvent> OnObjectAdded;
        static EngineEvent<GameObjectRemovedEvent> OnObjectRemoved;

        static EngineEvent<GameObjectTreeChanged> OnTreeChanged;
       
        static List<GameObject> gameObjects = new List<GameObject>();

        public static GameObject[] GetGameObjects() => gameObjects.ToArray();

        public static void RegisterGameobject(GameObject gameObject) 
        {
            gameObjects.Add(gameObject);
            OnObjectAdded?.Invoke(new GameObjectAddedEvent()
            {
                AddedGameObject = gameObject,
                TotalGameObjects = gameObjects.ToArray()
            });

            AlertTreeChange(gameObject);
        }

        public static void RegisterGameobjectGroup(GameObject[] _gameObjects)
        {
           foreach(GameObject o in _gameObjects)
            {
                RegisterGameobject(o);
            }
        }

        public static void DispatchGameobject(GameObject gameObject) 
        {
            gameObjects.Remove(gameObject);

            gameObject.ClearComponents();

            OnObjectRemoved?.Invoke(new GameObjectRemovedEvent()
            {
                RemovedGameObject = gameObject,
                TotalGameObjects = gameObjects.ToArray()
            });

            AlertTreeChange(gameObject);
        }

        public static void DispatchGameobjectGroup(GameObject[] _gameObjects)
        {
            foreach (GameObject o in _gameObjects) 
            {
                DispatchGameobject(o);
            }
        }

        public static void ClearAll()
        {
            while(gameObjects.Count > 0) 
            {
                DispatchGameobject(gameObjects[0]);
            }
        }

        public static void AlertTreeChange(GameObject changed)
        {
            OnTreeChanged?.Invoke(new GameObjectTreeChanged()
            {
                ChangedObject = changed,
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

    public class GameObjectAddedEvent : GameObjectEvent 
    {
        public GameObject AddedGameObject;
    }
    public class GameObjectRemovedEvent : GameObjectEvent 
    {
        public GameObject RemovedGameObject;
    }

    public class GameObjectTreeChanged : GameObjectEvent
    {
        public GameObject ChangedObject;
    }
}
