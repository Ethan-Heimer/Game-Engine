
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
using GameEngine.Engine.Settings;
using System.Diagnostics;
using System.Threading;

namespace GameEngine
{
    [ContainsEvents]
    [ContainsSettings]
    public static class SceneManager
    {
        [EngineSettings("Game")]
        static string StartingScene = "";

        static readonly EngineEvent<SceneLoadedEvent> OnSceneLoaded;

        static readonly EngineEvent<SceneUnloadedEvent> OnSceneUnloaded;
        static readonly EngineEvent<SceneSavedEvent> OnSceneSaved;

        static Dictionary<string, string> scenes = new Dictionary<string, string>();
        static Scene _currentScene;
        public static Scene currentScene
        {
            get { return _currentScene; }

            set
            {
                if (_currentScene != null)
                {
                    UnloadScene();
                    OnSceneUnloaded?.Invoke(new SceneUnloadedEvent() { Scene = value });
                }

                _currentScene = value;
                _currentScene.Load();
                OnSceneLoaded?.Invoke(new SceneLoadedEvent() { Scene = value });
            }
        }

        public static void Init()
        {
            FindScenes();
            LoadFirst();

            EngineEventManager.AddEventListener<OnEnterPlayMode>(e => HandleSceneOnPlay());
            EngineEventManager.AddEventListener<OnEnterEditMode>(e => HandleSceneOnEdit());
        }

        public static void FindScenes()
        {
            scenes.Clear();

            string[] scenesFound = AssetManager.FindFiles(".scene").Select(x => AssetManager.GetFilePath(x)).ToArray();
            foreach (var o in scenesFound)
            {
                string name = Path.GetFileNameWithoutExtension(o);
                scenes.Add(name, o);
            }
        }

        /*
        public static void LoadSceneAsync(string name)
        {
            Thread thread = new Thread(() =>
            {
                LoadScene(name);
            });

            thread.Start();
            thread.Join();
        }
        */

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
            string sceneName = Path.GetFileNameWithoutExtension(name);

            currentScene.name = sceneName;

            AssetManager.SaveFile(currentScene, name);

            Console.WriteLine(name);

            if (!scenes.ContainsKey(sceneName))
                scenes.Add(sceneName, name);

            OnSceneSaved?.Invoke(new SceneSavedEvent { Scene = currentScene });
        }

        static void LoadFirst()
        {
            try
            {
                if (currentScene != null)
                    LoadScene(currentScene);
                else
                    LoadScene(StartingScene);
            }
            catch
            {
                LoadScene(new Scene());
            }
        }

        static void HandleSceneOnPlay()
        {
            currentScene.Save();
            TempFileHandler.Serialize(currentScene, "temp.scene");
        }

        static void HandleSceneOnEdit()
        {
            Scene tempScene = (Scene)TempFileHandler.Deserialize("temp.scene");

            if (tempScene != null)
                LoadScene(tempScene);
        }

        //new thread?
        static void UnloadScene()
        {
            if (currentScene != null)
                GameObjectManager.ClearAll();
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