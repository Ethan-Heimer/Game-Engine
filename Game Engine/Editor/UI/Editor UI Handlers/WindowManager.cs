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
        public static Window CreateWindow(EditorWindow editorWindow)
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

            ShowWindow(window);

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
