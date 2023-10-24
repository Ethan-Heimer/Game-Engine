using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.Windows
{
    public class TestWindow : EditorWindow
    {
        public TestWindow() 
        {
            Width = 500;
            Height = 500;
        }

        public override void OnGUI(EditorGUIDrawer drawer)
        {
            drawer.Label("Hello World");
        }
    }
}
