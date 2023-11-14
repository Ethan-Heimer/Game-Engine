using GameEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GameEngine.Engine.Events;

namespace GameEngine.Editor.Windows
{
    [OpenWindowByDefault]
    public class HiarchyWindow : EditorWindow
    {
        public HiarchyWindow() 
        {
            Width = 300;
            Height = 600;

        }

        public override void OnGUI(EditorGUIDrawer drawer)
        {
            drawer.DrawLabel("Hiarchy");

            EngineEventManager.AddEventListener<SceneLoadedEvent>((e) => Hook(e.Scene, drawer));
        }

        void Hook(Scene scene, EditorGUIDrawer drawer)
        {
            scene.OnNewObjectLoaded += (g) => RenderGUI(scene, drawer);
        }

        void RenderGUI(Scene scene, EditorGUIDrawer drawer) 
        {
            Console.WriteLine("Hello");
            foreach(GameObject o in scene.GetGameobjects())
            {
                drawer.DrawText("Gameobject");
            }
        }
    }
}
