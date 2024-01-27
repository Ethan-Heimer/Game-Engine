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
            string[] sections = EngineSettings.Sections;

            foreach(string section in sections) 
            {
                ElementStyle header = ElementStyle.DefaultTextStyle
                    .OverrideFontSize(ElementStyle.LargeTextSize)
                    .OverridePadding(new System.Windows.Thickness(0, 10, 10, 10));

                drawer.DrawText(section, header);

                Setting[] settings = EngineSettings.GetSettings(section);
                
                foreach(var o in settings)
                {
                    ElementStyle textStyle = ElementStyle.DefaultTextStyle
                        .OverrideFontSize(ElementStyle.MediumTextSize)
                        .OverridePadding(new System.Windows.Thickness(10, 0, 0, 0));

                    drawer.DrawText(o.Name.Annunciated(), textStyle);

                    Type dataType = o.DataType();
                    PropertyInfo property = o.GetType().GetProperty("Value");

                    IFieldTemplate template = factory.TryGetTemplate(property, dataType, o);
                    template.Display(drawer);
                }
            }
        }

        
    }
}
