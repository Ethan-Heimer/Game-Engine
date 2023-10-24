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


        public static void Open()
        {
            WindowManager.Initalize();
            OpenEditor();
            playMode = false;
        }

        async static void OpenEditor()
        {
            /*
            var signal = new SemaphoreSlim(0, 1);

            var Window = WindowManager.Test();
            Window.Closed += (s, _) => signal.Release();

            Window.Show();

            await signal.WaitAsync();
            */
        }

        static Scene cachedScene = null;
        static SemaphoreSlim playSignal = null; 
        public async static void EnterPlayMode()
        {
            playMode = true;
            playSignal = new SemaphoreSlim(0, 1);
            cachedScene = SceneManager.currentScene;

            using(Scene runtimeScene = cachedScene.Clone() as Scene)
            {
                SceneManager.LoadScene(runtimeScene);
                await playSignal.WaitAsync();
            }
        }

        public static void ExitPlayMode() 
        {
            playMode = false;

            if (cachedScene != null)
            {
                SceneManager.LoadScene(cachedScene);
                cachedScene = null;
                playSignal.Release();
            }
        }
    }
}
