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

namespace GameEngine.Editor.Windows
{
    [OpenWindowByDefault]
    public class HiarchyWindow : EditorWindow
    {
        List<IFieldTemplate> fields = new List<IFieldTemplate>();
        UITemplateFactory<HiarchyFieldTemplateAttribute> factory = new UITemplateFactory<HiarchyFieldTemplateAttribute>(typeof(ComponentFieldBinder<>));

        public HiarchyWindow() 
        {
            Width = 300;
            Height = 600;
        }

        public override void OnGUI(EditorGUIDrawer drawer)
        {
            EngineEventManager.AddEventListener<GameObjectAddedEvent>((e) =>
            {
                var roots = GetRootObjects(e.TotalGameObjects);
                RenderGUI(roots, drawer);
            });
            EngineEventManager.AddEventListener<GameObjectRemovedEvent>((e) =>
            {
                var roots = GetRootObjects(e.TotalGameObjects);
                RenderGUI(roots, drawer);
            });
            EngineEventManager.AddEventListener<GameObjectTreeChanged>((e) =>
            {
                var roots = GetRootObjects(e.TotalGameObjects);
                RenderGUI(roots, drawer);
            });

            RenderGUI(GameObjectManager.GetGameObjects(), drawer);

            drawer.AddContextItem("New Game Object", (s, a) => CreateGameObject());
        }

        void RenderGUI(GameObject[] objects, EditorGUIDrawer drawer) 
        {
            drawer.Clear();
            fields.Clear();

            drawer.DrawText("Tree", ElementStyle.DefaultTextStyle.OverrideFontSize(35).OverrideMargin(new System.Windows.Thickness(10)));

            foreach (GameObject o in objects)
            {
                DrawGameObjectUI(o, drawer);
            }
        }

        public override void OnUpdateGUI(EditorGUIDrawer drawer)
        {
            fields.ForEach(x => x.Update(drawer));
        }

        void DrawGameObjectUI(GameObject obj, EditorGUIDrawer drawer)
        {
            IFieldTemplate template = factory.TryGetTemplate(obj.GetType().GetField("Name"), obj);
            if(template == null)
                return;

            fields.Add(template);
            template.Display(drawer);

            Console.WriteLine(obj.GetChildren().Length); 


            drawer.Indent();
            foreach(var o in obj.GetChildren())
            {
                DrawGameObjectUI(o, drawer);
            }
            drawer.Outdent();
        }

        GameObject[] GetRootObjects(GameObject[] objs)
        {
            return objs.Where(x =>
            {
                Console.WriteLine(x.parent + " parent");
                return x.parent == null;
            }).ToArray();
        }

        void CreateGameObject()
        {
            GameObject gameObect = new GameObject();
            gameObect.Transform.Position = Vector2.One * 500;
            gameObect.AddComponent<TextureRenderer>().Path = "PlaceHolderTwo";
            gameObect.AddComponent<Move>().Speed = 9;
            gameObect.AddComponent<TestComponent>();

            GameObject child = new GameObject();
            child.AddComponent<TextureRenderer>().Path = "PlaceHolderOne";
            child.AddComponent<Move>().Speed = 9;
            child.AddComponent<TestComponent>();

            gameObect.AddChild(child);
        }
    }
}
