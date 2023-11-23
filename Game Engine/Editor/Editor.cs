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
using GameEngine.Engine.Rendering;
using System.IO;
using System.Windows;
using System.Diagnostics;

namespace GameEngine.Editor
{
    public static class Editor
    {
        static EditorCamera editorCamera = new EditorCamera();
        static bool Initialized;
        public static void Start()
        {
            Game1.AfterInit += Open;
        }

       
        static void Open()
        {
            EngineEventManager.AddEventListener<OnEnterEditMode>((e) => SetToEditorCamera());
           

            PlayModeManager.SetMode(PlayModeManager.PlayMode.Edit);

            Grid grid = new Grid();
            EngineEventManager.AddEventListener<WhileInEditMode>((e) => grid.Draw(new Microsoft.Xna.Framework.Rectangle()
            {
                Width = CameraManager.GetVisableArea().Width + 100,
                Height = CameraManager.GetVisableArea().Height + 100,

                X = CameraManager.GetVisableArea().X,
                Y = CameraManager.GetVisableArea().Y
            }, 100));

            DefaultWindowHandler.OpenDefaultWindows();

            Initialized = true;
        }

        static void SetToEditorCamera() => CameraManager.SetMainCamera(editorCamera);

        
    }
}
