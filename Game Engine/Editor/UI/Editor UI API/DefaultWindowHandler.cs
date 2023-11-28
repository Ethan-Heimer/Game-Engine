using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.Windows
{
    public static class DefaultWindowHandler
    {
        public static void OpenDefaultWindows()
        {
            List<EditorWindow> defaultWindows = FindDefaultWindows();
            defaultWindows.ForEach(x => WindowManager.CreateWindow(x));
        }

        static List<EditorWindow> FindDefaultWindows()
        {
            List<EditorWindow> windows = new List<EditorWindow>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(EditorWindow)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(EditorWindow)) && myType.GetCustomAttribute(typeof(OpenWindowByDefaultAttribute)) != null))
            {
                windows.Add((EditorWindow)Activator.CreateInstance(type, new object[0]));
            }

            return windows;
        }
    }

    public class OpenWindowByDefaultAttribute : Attribute { }
}
