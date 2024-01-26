using GameEngine.Editor.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace GameEngine.Editor.UI.Inspector
{
    public class Inspector
    {
        List<IFieldTemplate> fields = new List<IFieldTemplate>();
        UITemplateFactory<InspectingFieldTemplateAttribute> factory = new UITemplateFactory<InspectingFieldTemplateAttribute>(typeof(ComponentFieldBinder<>));

        public Inspector(GameObject gameObject, EditorGUIDrawer drawer)
        {
            DrawInspector(gameObject, drawer);
        }
        public void Update(EditorGUIDrawer drawer)
        {
            fields.ForEach(x =>
            {
                x.Update(drawer);
            });
        }

        void DrawInspector(GameObject gameObject, EditorGUIDrawer drawer)
        {
            drawer.StartHorizontalGroup();
                DrawIconField(gameObject, drawer);
                DrawNameField(gameObject, drawer);
            drawer.EndGroup();

            foreach (Component o in gameObject.GetAllComponents())
            {
                string componentName = o.BindingBehavior.GetType().Name.Spaced();

                var title = DrawHeading(componentName, drawer);
                ContextManager context = new ContextManager(title);
                context.AddOption("Remove Component", (e, s) => gameObject.RemoveComponent(o));

                foreach (FieldInfo f in o.GetFields())
                {
                    Drawfield(f, o, drawer);
                }
            }

            Console.WriteLine("All Components Rendered");
        }

        TextBlock DrawHeading(string text, EditorGUIDrawer drawer)
        {
            return drawer.DrawText(text, new ElementStyle()
            {
                FontSize = ElementStyle.MediumTextSize,
                Background = ElementStyle.TertiaryBackgroundColor,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Padding = new Thickness(10, 2, 0, 2),
                Margin = new Thickness(0, 5, 0, 5),
                DynamicSize = true,
            });
        }

        void Drawfield(FieldInfo f, Component c, EditorGUIDrawer drawer)
        {
            IFieldTemplate template = factory.TryGetTemplate(f, c.BindingBehavior);

            if (template == null)
                return;

            template.Display(drawer);

            fields.Add(template);
        }

        void DrawNameField(GameObject gameObject, EditorGUIDrawer drawer)
        {
            FieldInfo name = gameObject.GetType().GetField("Name");

            IFieldTemplate template = factory.TryGetTemplate(name, gameObject);
            template.Display(drawer);

            fields.Add(template);
        }

        void DrawIconField(GameObject gameObject, EditorGUIDrawer drawer)
        {
            drawer.ClearContextMenu();

            FieldInfo icon = gameObject.GetType().GetField("Icon");

            IFieldTemplate template = factory.TryGetTemplate(icon, gameObject);
            template.Display(drawer);

            fields.Add(template);
        }
    }
}
