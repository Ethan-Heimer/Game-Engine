
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GameEngine.Editor;

namespace GameEngine
{
    public static class SceneManager
    {
        public static event Action<Scene> OnSceneUnloaded;
        public static event Action<Scene> OnSceneLoaded;
        public static event Action<Scene> OnSceneSaved;

        static Dictionary<string, string> scenes = new Dictionary<string, string>(); 

        static Scene _currentScene;
        public static Scene currentScene
        {
            get{return _currentScene;}

            set
            {
                if (_currentScene != null)
                {
                    OnSceneUnloaded?.Invoke(_currentScene);
                    GameObject.OnGameObjectCreated -= currentScene.LoadGameobject;
                }
                
                _currentScene = value;
                GameObject.OnGameObjectCreated += currentScene.LoadGameobject;
                OnSceneLoaded?.Invoke(value);
            }
        }

        public static void LoadScenes() 
        {
            scenes.Clear();

            string[] scenesFound = AssetManager.FindFiles(".scene").Select(x => AssetManager.GetFilePath(x)).ToArray();
            foreach (var o in scenesFound)
            {
                string name = Path.GetFileNameWithoutExtension(o);
                Console.WriteLine(name);
                scenes.Add(name, o);
            }
            
            try
            {
                if (currentScene != null)
                    LoadScene(currentScene);
                else
                    LoadScene(scenes.First().Key);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                LoadScene(new Scene());
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
            string name = AssetManager.GetFilePath(path);

            currentScene.name = Path.GetFileNameWithoutExtension(name);
            Console.WriteLine(currentScene.name + " Name");

            AssetManager.SaveFile(currentScene, name); 
            OnSceneSaved?.Invoke(currentScene);

            LoadScenes(); 
        }
    }
}