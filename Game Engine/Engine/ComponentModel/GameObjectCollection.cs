using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.ComponentManagement
{
    [Serializable]
    public class GameObjectCollection : ISerializable, IEnumerable<GameObject>, ICloneable
    {
        public event Action<GameObject> onGameObjectAdded;
        public event Action<GameObject> onGameObjectRemoved;    

        public event Action<Behavior> onGameObjectComponentAdded;
        public event Action<Behavior> onGameObjectComponentRemoved;

        List<GameObject> gameObjects = new List<GameObject>();

        public GameObjectCollection(){}

        public GameObjectCollection(SerializationInfo info, StreamingContext context)
        {
            gameObjects = (List<GameObject>)info.GetValue("GameObjects", typeof(List<GameObject>));
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            int i = 0;
            foreach (GameObject o in gameObjects)
            {
                i++;
                Console.WriteLine(i);
                o.OnComponentAdded += (behavior) => onGameObjectComponentAdded?.Invoke(behavior);
                o.OnComponentRemoved += (behavior) => onGameObjectComponentRemoved?.Invoke(behavior);
            }
        }

        public void Clear() => gameObjects.Clear();

        public void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
            onGameObjectAdded?.Invoke(gameObject);

            gameObject.OnComponentAdded += (behavior) => onGameObjectComponentAdded?.Invoke(behavior);
            gameObject.OnComponentRemoved += (behavior) => onGameObjectComponentRemoved?.Invoke(behavior);

            Console.WriteLine(gameObjects.Count);
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            gameObjects.Remove(gameObject); 
            onGameObjectRemoved?.Invoke(gameObject);

            gameObject.OnComponentAdded -= (behavior) => onGameObjectComponentAdded?.Invoke(behavior);
            gameObject.OnComponentRemoved -= (behavior) => onGameObjectComponentRemoved?.Invoke(behavior);
        }

        public Component[] GetComponents()
        {
            List<Component> components = new List<Component>();
            gameObjects.ForEach(x => components.AddRange(x.GetAllComponents()));
            
            return components.ToArray();    
        }

        public IEnumerator<GameObject> GetEnumerator()
        {
            return new GameObjectEnum(gameObjects.ToArray());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public object Clone()
        {
            GameObjectCollection collection = new GameObjectCollection();

            for(int i = 0; i < gameObjects.Count; i++)
                collection.AddGameObject(gameObjects[i].Clone() as GameObject);
            
            return collection;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("GameObjects", gameObjects);
        }
    }

    public class GameObjectEnum : IEnumerator<GameObject>
    {
        public GameObject[] gameObjects;

        int position = -1;

        public GameObjectEnum(GameObject[] objects)
        {
            this.gameObjects = objects;
        }
        public bool MoveNext()
        {
            position++;
            return (position < gameObjects.Length);
        }
        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }
        public GameObject Current
        {
            get
            {
                try
                {
                    return gameObjects[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Dispose() { }
    }
}
