using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace GameEngine.Editor.Windows
{
    public abstract class EditorWindow : Window
    {
        public abstract void OnGUI(EditorGUIDrawer drawer);
        public virtual void OnUpdateGUI(EditorGUIDrawer drawer) { }
    }
}
