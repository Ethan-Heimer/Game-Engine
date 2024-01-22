using GameEngine.Editor.Windows;
using GameEngine.Engine.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.UI
{
    [OpenWindowByDefault]
    public class SettingsWindow : EditorWindow
    {
        public SettingsWindow()
        {
            Width = 500;
            Height = 500;

            RelativePosition = RelativeWindowPosition.Float;
        }

        public override void OnGUI(EditorGUIDrawer drawer)
        {
            var sections = EngineSettings.GetSections();
            foreach (var section in sections) 
            {
                drawer.DrawText(section.Title);
            }
        }
    }
}
