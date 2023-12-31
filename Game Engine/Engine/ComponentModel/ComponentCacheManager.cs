﻿using GameEngine.Engine;
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

                if (o.CanExecuteAlways)
                {
                    cache.Add(o.FunctionName + "InEditor", new BehaviorMethodCollection(o));
                }
            }

            EngineEventManager.AddEventListener<GameObjectAddedEvent>(AddCache);
            EngineEventManager.AddEventListener<GameObjectRemovedEvent>(RemoveCache);
        }

        public static void AddCache(GameObjectAddedEvent e)
        {
            AddCache(e.AddedGameObject);
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

                if (o.CanExecuteAlways && ExecuteAlways(behavior))
                    cache[o.FunctionName + "InEditor"].TryCacheBehavior(behavior);
            }

            addedArgs.behaviorAdded = behavior;
            OnCompnentAddedEvent?.Invoke(addedArgs);
        }

 
        public static void RemoveCache(GameObjectRemovedEvent e)
        {
            RemoveCache(e.RemovedGameObject);
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

                if (o.CanExecuteAlways && ExecuteAlways(c))
                    cache[o.FunctionName + "InEditor"].TryCacheBehavior(c);
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

        static bool ExecuteAlways(Behavior behavior)
        {
            return behavior.GetType().GetCustomAttribute(typeof(ExecuteAlwaysAttribute)) != null;
          
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
