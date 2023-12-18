using GameEngine.Debugging;
using GameEngine.Editor.Windows;
using GameEngine.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GameEngine.Editor
{
    [Note(note = "Time between window draws can be a engine setting in a json file / Drawer in update might be able to be cashed")]
    [ContainsEvents]
    public static class WindowManager
    {
        static List<EditorWindow> windows = new List<EditorWindow>();
        static Dictionary<EditorWindow, EditorGUIDrawer> drawers = new Dictionary<EditorWindow, EditorGUIDrawer>();

        public static void Init()
        {
           OpenDefaultWindows();
            EngineEventManager.AddEventListener<OnEngineTickEvent>(e => Update());
        }
        public static void OpenDefaultWindows()
        {
            List<EditorWindow> defaultWindows = FindDefaultWindows();
            defaultWindows.ForEach(x => CreateWindow(x));
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

        public static Window CreateWindow(EditorWindow window)
        {
            //window type should be passed, not refrence value

            window.Title = StringSpacer.Space(window.GetType().Name);

            window.Style = (Style)Application.Current.Resources["Window"];

            StackPanel panel = new StackPanel();
            panel.Name = "Content";

            window.Content = panel;
            
            EditorGUIDrawer drawer = new EditorGUIDrawer(window, panel);
            window.OnGUI(drawer);

            drawers.Add(window, drawer);
            
            ShowWindow(window);

            return window;
        }

        static async void ShowWindow(EditorWindow window)
        {
            var signal = new SemaphoreSlim(0, 1);

            window.Closed += (s, _) => signal.Release();
            window.Closed += (s, _) => drawers.Remove(window);

            window.Show();

            windows.Add(window);

            await signal.WaitAsync();

            windows.Remove(window);
        }


        static long deley;
        static void Update()
        {
            var time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if(time > deley)
            {
                foreach(var o in windows) 
                {
                    var drawer = drawers[o];
                    o.OnUpdateGUI(drawer);
                }

                deley = time + 100;
            }
        }

    }

    public class OpenWindowByDefaultAttribute : Attribute { }
}
