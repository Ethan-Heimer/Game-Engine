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
using Microsoft.Xna.Framework;
using GameEngine.Pointer;
using GameEngine.Editor.Widgets;

namespace GameEngine.Editor
{
    public static class Editor
    {
        static EditorCamera editorCamera = new EditorCamera();
        static Grid grid = new Grid();
        static GameObjectPointer editorPointer = new GameObjectPointer();

        public static void Start()
        {
            Game1.AfterInit += Open;
        }

       
        static void Open()
        {
            EngineEventManager.AddEventListener<OnEnterEditMode>((e) => SetToEditorCamera());
            EngineEventManager.AddEventListener<WhileInEditMode>((e) => Update());

            DefaultWindowHandler.OpenDefaultWindows();
            PlayModeManager.SetMode(PlayModeManager.PlayMode.Edit);

            WidgetDrawer.Init();

            editorPointer.AddManipulator(new OnGameObjectSelectManipulator())
            .AddManipulator(new GameObjectTranslateManipulator())
            .AddManipulator(new GameObjectRotationManipulator())
            .AddManipulator(new GameObjectScaleManipulator());
        }

        static void Update()
        {
            grid.Draw(new Rectangle()
            {
                Width = CameraManager.GetVisableArea().Width + 100,
                Height = CameraManager.GetVisableArea().Height + 100,

                X = CameraManager.GetVisableArea().X,
                Y = CameraManager.GetVisableArea().Y
            }, 100);

            editorPointer.Update();
        }

        static void SetToEditorCamera() => CameraManager.SetMainCamera(editorCamera);
    }
}
