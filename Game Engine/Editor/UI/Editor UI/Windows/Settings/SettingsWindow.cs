using GameEngine.Editor.UI.Inspector;
using GameEngine.Editor.Windows;
using GameEngine.Engine.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.UI
{
    [OpenWindowByDefault]
    public class SettingsWindow : EditorWindow
    {
        UITemplateFactory<InspectingFieldTemplateAttribute> factory = new UITemplateFactory<InspectingFieldTemplateAttribute>(typeof(ComponentFieldBinder<>));

        public SettingsWindow()
        {
            Width = 500;
            Height = 500;

            RelativePosition = RelativeWindowPosition.Float;
        }

        public override void OnGUI(EditorGUIDrawer drawer)
        {
            /*
            var sections = EngineSettings.GetSections();

            foreach (var section in sections) 
            {
                drawer.DrawText(section.Title);

                foreach(var option in  section.Values) 
                {
                    drawer.DrawText(option.Option);

                    Type fieldType = Type.GetType(option.Type);
                    PropertyInfo property = option.GetType().GetProperty("Value");

                    IFieldTemplate template = factory.TryGetTemplate(property, fieldType, option);

                    if (template == null)
                        return;
                }
            }

            */
        }

        
    }
}
