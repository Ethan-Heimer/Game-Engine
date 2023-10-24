using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameEngine.Editor.Windows
{
    public abstract class EditorWindow
    {
        public int Width = 0;
        public int Height = 0;

        public abstract void OnGUI(EditorGUIDrawer drawer);
    }
}
