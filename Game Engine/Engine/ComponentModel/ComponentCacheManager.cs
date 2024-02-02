using GameEngine.Engine;
using GameEngine.Engine.ComponentModel;
using GameEngine.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace GameEngine.ComponentManagement
{
    [ContainsEvents]
    public static class ComponentCacheManager
    {
        static Dictionary<string, BehaviorMethodCollection> cache;

        static EngineEvent<OnComponentAdded> OnCompnentAddedEvent;
        static OnComponentAdded addedArgs = new OnComponentAdded();

        static EngineEvent<OnComponentRemoved> OnComponentRemovedEvent;
        static OnComponentRemoved removeArgs = new OnComponentRemoved();

        public static void Init()
        {
            cache = new Dictionary<string, BehaviorMethodCollection>();
            foreach (var o in BehaviorFunctions.functionTypes)
            {
                cache.Add(o.FunctionName, new BehaviorMethodCollection(o));
            }

            EngineEventManager.AddEventListener<GameObjectsAddedEvent>(AddCache);
            EngineEventManager.AddEventListener<GameObjectsRemovedEvent>(RemoveCache);
        }

        public static void AddCache(GameObjectsAddedEvent e)
        {
            foreach (var o in e.AddedGameObjects)
            {
                AddCache(o);
            }
        }
        public static void AddCache(GameObject e)
        {
            foreach (var c in e.GetAllComponents())
            {
                AddCache(c.BindingBehavior);
            }
        }
        public static void AddCache(Behavior behavior)
        {
            foreach (var o in BehaviorFunctions.functionTypes)
            {
                cache[o.FunctionName].TryCacheBehavior(behavior);
            }

            addedArgs.behaviorAdded = behavior;
            OnCompnentAddedEvent?.Invoke(addedArgs);

            Console.WriteLine(behavior.GetType().Name);
        }

 
        public static void RemoveCache(GameObjectsRemovedEvent e)
        {
            foreach(var o in e.RemovedGameObjects)
            {
                RemoveCache(o);
            }
        }
        public static void RemoveCache(GameObject e)
        {
            foreach (var c in e.GetAllComponents())
            {
                RemoveCache(c.BindingBehavior);
            }
        }
        public static void RemoveCache(Behavior c)
        {
            foreach (var o in BehaviorFunctions.functionTypes)
            {
                cache[o.FunctionName].TryRemoveBehavior(c);
            }

            removeArgs.behaviorRemoved = c;
            OnComponentRemovedEvent?.Invoke(removeArgs);
        }

        public static void ClearCache() 
        {
            foreach(var o in cache)
            {
                o.Value.ClearCache();
            }
        }

        public static BehaviorMethodCollection GetCache(string name)
        {
            return cache[name];
        }
    }

    public struct OnComponentAdded : IEventArgs
    {
        public Behavior behaviorAdded;
        public object Sender
        {
            get;
            set;
        }
    }

    public struct OnComponentRemoved : IEventArgs
    {
        public Behavior behaviorRemoved;
        public object Sender
        {
            get;
            set;
        }
    }
}
