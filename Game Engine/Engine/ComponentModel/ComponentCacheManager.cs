using GameEngine.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameEngine.ComponentManagement
{
    public static class ComponentCacheManager
    {
        static Dictionary<string, BehaviorMethodCollection> _cache;
        static Dictionary<string, BehaviorMethodCollection> cache
        {
            get
            {
                if(_cache == null)
                {
                    InitalizeCasheManager();
                }

                return _cache;
            }
        }

        public static void AddCache(GameObject gameObject)
        {
            foreach(var c in gameObject.GetAllComponents())
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
        }

        public static void RemoveCache(GameObject gameObject)
        {
            foreach (var c in gameObject.GetAllComponents())
            {
                RemoveCache(c.BindingBehavior);
            }
        }

        private static void RemoveCache(Behavior c)
        {
            foreach (var o in BehaviorFunctions.functionTypes)
            {
                cache[o.FunctionName].TryRemoveBehavior(c);
            }
        }

        public static void ClearCache() 
        {
            foreach(var o in cache)
            {
                o.Value.ClearCache();
            }
        }

        static void PopulateCache(Scene scene)
        {
            foreach (GameObject o in scene.GetGameObjectCollection())
                AddCache(o);
        }

        static void OnSceneUnload(SceneUnloadedEvent e)
        {
            ClearCache();

            e.Scene.GetGameObjectCollection().onGameObjectAdded -= AddCache;
            e.Scene.GetGameObjectCollection().onGameObjectRemoved -= RemoveCache;

            e.Scene.GetGameObjectCollection().onGameObjectComponentAdded -= AddCache;
            e.Scene.GetGameObjectCollection().onGameObjectComponentRemoved -= RemoveCache;
        }

        static void OnSceneLoad(SceneLoadedEvent e)
        {
            PopulateCache(e.Scene);

            e.Scene.GetGameObjectCollection().onGameObjectAdded += AddCache;
            e.Scene.GetGameObjectCollection().onGameObjectRemoved += RemoveCache;

            e.Scene.GetGameObjectCollection().onGameObjectComponentAdded += AddCache;
            e.Scene.GetGameObjectCollection().onGameObjectComponentRemoved += RemoveCache;
        }

        public static BehaviorMethodCollection GetCache(string name)
        {
            return cache[name];
        }

        static void InitalizeCasheManager()
        {
            _cache = new Dictionary<string, BehaviorMethodCollection>();
            foreach (var o in BehaviorFunctions.functionTypes)
            {
                cache.Add(o.FunctionName, new BehaviorMethodCollection(o));
            }

            PopulateCache(SceneManager.currentScene);

            EngineEventManager.AddEventListener<SceneLoadedEvent>(OnSceneLoad);
            EngineEventManager.AddEventListener<SceneUnloadedEvent>(OnSceneUnload);
        }
    }
}
