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
using System.ComponentModel;
using GameEngine.Editor.UI.Inspector;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using GameEngine.Editor.UI.Hiarchy;
using GameEngine.Debugging;

namespace GameEngine.Editor.Windows
{
    [OpenWindowByDefault]
    [Note(note = "To give gameobjects some structure, it might be a good idea to make a scene object that acts as the root node of the gameobject tree. every gameobject is a child of the root, and code might be cleaner because objects would have better structure")]
    public class HiarchyWindow : EditorWindow
    {
        Hiarchy hiarchy = new Hiarchy();

        public HiarchyWindow() 
        {
            Width = 300;
            Height = 600;
        }

        public override void OnGUI(EditorGUIDrawer drawer)
        {
            EngineEventManager.AddEventListener<GameObjectAddedEvent>((e) => Draw(drawer));
            EngineEventManager.AddEventListener<GameObjectRemovedEvent>((e) => Draw(drawer));
            EngineEventManager.AddEventListener<GameObjectTreeChanged>((e) => Draw(drawer));

            Draw(drawer);
        }

        void Draw(EditorGUIDrawer drawer)
        {
            GameObject[] objects = GetRootObjects();
            hiarchy.Draw(objects, drawer);
        }

        public override void OnUpdateGUI(EditorGUIDrawer drawer)
        {
            hiarchy.Update(drawer);
        }

        GameObject[] GetRootObjects()
        {
            GameObject[] objs = GameObjectManager.GetGameObjects();

            return objs.Where(x =>
            {
                Console.WriteLine(x.Parent + " parent");
                return x.Parent == null;
            }).ToArray();
        }

    }
}
