using GameEngine.Editor.UI.Inspector;
using GameEngine.Editor.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.UI.Hiarchy
{
    public class Hiarchy
    {
        List<IFieldTemplate> fields = new List<IFieldTemplate>();
        UITemplateFactory<HiarchyFieldTemplateAttribute> factory = new UITemplateFactory<HiarchyFieldTemplateAttribute>(typeof(ComponentFieldBinder<>));

        public void Draw(GameObject[] objects, EditorGUIDrawer drawer)
        {
            drawer.Clear();
            drawer.ClearContextMenu();
            fields.Clear();

            drawer.DrawText("Tree", ElementStyle.DefaultTextStyle.OverrideFontSize(35).OverrideMargin(new System.Windows.Thickness(10)));

            foreach (GameObject o in objects)
            {
                DrawGameObjectUI(o, drawer, 0);
            }

            drawer.AddContextItem("New Game Object", (s, a) => CreateGameObject());
        }

        public void Update(EditorGUIDrawer drawer)
        {
            fields.ForEach(x => x.Update(drawer));
        }

        void DrawGameObjectUI(GameObject obj, EditorGUIDrawer drawer, int indentLevel)
        {
            IFieldTemplate nameTemplate = factory.TryGetTemplate(obj.GetType().GetField("Name"), obj);
            IFieldTemplate iconTemplate = factory.TryGetTemplate(obj.GetType().GetField("Icon"), obj);

            fields.Add(nameTemplate);
            fields.Add(iconTemplate);

            ElementStyle groupStyle = ElementStyle.DefaultGroupStyle;

            groupStyle.HoverEfects = true;
            groupStyle.OnHoverBackground = ElementStyle.AccentBackgroundColor;

            var group = drawer.StartHorizontalGroup(groupStyle);
            iconTemplate.Display(drawer, indentLevel);
            nameTemplate.Display(drawer);
            drawer.EndGroup();

            ContextManager contextManager = new ContextManager(group);
            contextManager.AddOption("Add Child", (e, s) => CreateChild(obj))
            .AddOption("Delete", (e, s) => DeleteObject(obj))
            .AddOption("Duplicate", (e, s) => DuplicateObject(obj));

            foreach (var o in obj.GetChildren())
            {
                DrawGameObjectUI(o, drawer, indentLevel + 1);
            }
        }

        public GameObject CreateGameObject()
        {
            GameObject gameObect = new GameObject();

            return gameObect;
        }

        void CreateChild(GameObject parent)
        {
            GameObject child = CreateGameObject();
            parent.AddChild(child);
        }

        void DeleteObject(GameObject obj) => obj.Destroy();

        void DuplicateObject(GameObject obj)
        {
            obj.Clone();
        }
    }
}
