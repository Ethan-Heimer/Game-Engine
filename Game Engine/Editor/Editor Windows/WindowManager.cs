using GameEngine.Editor.Windows;
using GameEngine.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GameEngine.Editor
{
    public static class WindowManager
    {
        public static void Initalize()
        {
            App app = new App();
            app.InitializeComponent();

            List<EditorWindow> editorWindows = FindWindows();
            List<Window> windows = InstanciateWindows(editorWindows);

            ShowWindows(windows);
        }

        public static void ShowWindows(List<Window> windows)
        {
            foreach(Window o in windows) 
            {
                ShowWindow(o);
            }
        }

        static List<EditorWindow> FindWindows()
        {
            List<EditorWindow> windows = new List<EditorWindow>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(EditorWindow)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(EditorWindow))))
            {
                windows.Add((EditorWindow)Activator.CreateInstance(type, new object[0]));
            }
            
            return windows;
        }

        static List<Window> InstanciateWindows(List<EditorWindow> windows)
        {
            return windows.Select(x => CreateWindow(x)).ToList();
        }

        static Window CreateWindow(EditorWindow editorWindow)
        {
            var window = new Window();
            window.Title = StringSpacer.Space(editorWindow.GetType().Name);

            window.Width = editorWindow.Width;
            window.Height = editorWindow.Height;

            window.Style = (Style)Application.Current.Resources["Window"];

            StackPanel panel = new StackPanel();
            panel.Name = "Content";

            window.Content = panel;

            EditorGUIDrawer drawer = new EditorGUIDrawer(panel);

            editorWindow.OnGUI(drawer);

            return window;
        }

        static async void ShowWindow(Window window)
        {
            var signal = new SemaphoreSlim(0, 1);

            window.Closed += (s, _) => signal.Release();

            window.Show();

            await signal.WaitAsync();
        }
    }
}
