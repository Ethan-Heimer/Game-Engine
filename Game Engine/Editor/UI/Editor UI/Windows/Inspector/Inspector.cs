using GameEngine.Editor.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            DrawNameField(gameObject, drawer);

            foreach (Component o in gameObject.GetAllComponents())
            {
                DrawHeading(o.BindingBehavior.GetType().Name, drawer);

                foreach (FieldInfo f in o.GetFields())
                {
                    Drawfield(f, o, drawer);
                }
            }
        }

        void DrawHeading(string text, EditorGUIDrawer drawer)
        {
            drawer.DrawText(text, new ElementStyle()
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
    }
}
