using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameEngine.ComponentManagement;
using GameEngine.Editor.Windows;
using GameEngine.Engine;
using System.Reflection;
using GameEngine.Engine.Events;
using GameEngine.Editor.UI;
using GameEngine.Rendering;

namespace GameEngine.Editor
{
    public static class Editor
    {
        public static event Action OnPlayModeEnter;
        public static event Action OnPlayModeExit;

        static bool _playMode;
        public static bool playMode
        {
            get { return _playMode; }
            private set 
            { 
                _playMode = value;
                GameExecuter.play = value;

                if (value)
                    OnPlayModeEnter?.Invoke();
                else
                    OnPlayModeExit?.Invoke();
            }
        }

        public static void Start()
        {
            Game1.AfterInit += Open;
        }

       
        static void Open()
        {
            Grid grid = new Grid();
            EngineEventManager.AddEventListener<OnEngineDrawEvent>((e) => grid.Draw(e.drawer, CameraManager.GetVisableArea(), 100));

            Console.WriteLine(grid);
            playMode = false;

            DefaultWindowHandler.OpenDefaultWindows();
        }

        public static void EnterPlayMode()
        {
            BinarySeriailizer serializer = new BinarySeriailizer();
            playMode = true;

            SceneManager.currentScene.Save();
            serializer.Serialize(SceneManager.currentScene, "temp.scene");
        }

        public static void ExitPlayMode() 
        {
            BinarySeriailizer serializer = new BinarySeriailizer();

            playMode = false;
            var cachedScene = (Scene)serializer.Deserialize("temp.scene");

            Console.WriteLine(cachedScene.gameObjects.Length + " gameObjects");

            if (cachedScene != null)
            {
                SceneManager.LoadScene(cachedScene);
            }
        }
    }
}
