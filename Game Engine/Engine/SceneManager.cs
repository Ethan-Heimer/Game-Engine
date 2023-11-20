
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GameEngine.Editor;
using GameEngine.Engine.Events;
using GameEngine.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    [ContainsEvents]
    public static class SceneManager
    {
        static readonly EngineEvent<SceneLoadedEvent> OnSceneLoaded;

        static readonly EngineEvent<SceneUnloadedEvent> OnSceneUnloaded;
        static readonly EngineEvent<SceneSavedEvent> OnSceneSaved;

        static Dictionary<string, string> scenes = new Dictionary<string, string>(); 
        static Scene _currentScene;
        public static Scene currentScene
        {
            get{return _currentScene;}

            set
            {
                if (_currentScene != null)
                {
                    OnSceneUnloaded?.Invoke(new SceneUnloadedEvent() { Scene = value });
                    _currentScene.Unload();
                }
                
                _currentScene = value;
                OnSceneLoaded?.Invoke(new SceneLoadedEvent() { Scene = value });
                _currentScene.Load();
            }
        }

        public static void Init()
        {
            FindScenes();
            LoadFirst();
        }

        public static void FindScenes() 
        {
            scenes.Clear();

            string[] scenesFound = AssetManager.FindFiles(".scene").Select(x => AssetManager.GetFilePath(x)).ToArray();
            foreach (var o in scenesFound)
            {
                string name = Path.GetFileNameWithoutExtension(o);
                Console.WriteLine(name);
                scenes.Add(name, o);
            }
        }

        public static void LoadScene(string name)
        {
            string path = scenes[name];
            Scene scene = AssetManager.LoadFile<Scene>(path);

            LoadScene(scene);
        }

        public static void LoadScene(Scene scene)
        {
            currentScene = scene;
        }

        public static void SaveScene(string path)
        {
            currentScene.Save();
            
            string name = AssetManager.GetFilePath(path);

            currentScene.name = Path.GetFileNameWithoutExtension(name);
            Console.WriteLine(currentScene.name + " Name");

            AssetManager.SaveFile(currentScene, name);
            OnSceneSaved?.Invoke(new SceneSavedEvent { Scene = currentScene });
        }

        static void LoadFirst()
        {
            try
            {
                if (currentScene != null)
                    LoadScene(currentScene);
                else
                    LoadScene(scenes.First().Key);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                LoadScene(new Scene());
                Console.WriteLine("New Loaded");
            }
        }
    }

    public class SceneLoadedEvent : SceneEvent { }
    public class SceneUnloadedEvent : SceneEvent { }
    public class SceneSavedEvent : SceneEvent { }

    public class SceneEvent : IEventArgs
    {
        public object Sender
        {
            get; set;
        }

        public Scene Scene; 
    }
}