using GameEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GameEngine.Engine.Events;
using GameEngine.Engine;
using GameEngine.Game;
using Microsoft.Xna.Framework;

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
            EngineEventManager.AddEventListener<GameObjectAddedEvent>((e) => RenderGUI(e.TotalGameObjects, drawer));
            EngineEventManager.AddEventListener<GameObjectRemovedEvent>((e) => RenderGUI(e.TotalGameObjects,drawer));

            RenderGUI(GameObjectManager.GetGameObjects(), drawer);
        }

        void RenderGUI(GameObject[] objects, EditorGUIDrawer drawer) 
        {
            Console.WriteLine("gameObject Added");

            drawer.Clear();
            drawer.DrawLabel("Hiarchy");

            foreach(GameObject o in objects)
            {
                drawer.DrawText("Gameobject");
            }

            drawer.DrawButton("Create GameObject", (s, _) => CreateGameObject());
        }

        void CreateGameObject()
        {
            Random r = new Random();

            GameObject gameObect = new GameObject();
            gameObect.Transform.Position = Vector2.One * 500;
            gameObect.AddComponent<TextureRenderer>().Path = "PlaceHolderTwo";
            gameObect.AddComponent<Move>().Speed = 9;
            gameObect.AddComponent<TestComponent>();

            gameObect.Name = "Test Hoe";
        }
    }
}
