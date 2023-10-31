using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.Reflection;

namespace GameEngine.Editor.Windows
{
    public abstract class Component
    {
        public void Draw(EditorGUIDrawer editorGUI, params object[] args)
        {
            MethodInfo onDraw = this.GetType().GetMethod("OnDraw");

            if(onDraw == null)
            {
                throw new Exception("Component Does Not Define An OnDraw Method");
            }

            List<object> arguments = args.ToList();
            arguments.Insert(0, editorGUI);

            onDraw.Invoke(this, arguments.ToArray());
        }
    }
}
