using GameEngine.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using GameEngine.Debugging;
using System.Reflection;
using GameEngine.Editor.UI.Inspector;
using GameEngine.Engine;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.Serialization;
using Microsoft.Build.Tasks.Xaml;
using GameEngine.Engine.ComponentModel;
using System.Windows.Media;
using System.Windows.Controls;

namespace GameEngine.Editor.Windows
{
    [OpenWindowByDefault]
    [Note(note = "there Needs to be a way to tell when the ui needs to be rerendered")]
    public class InspectorWindow : EditorWindow
    {
        Inspector inspector;
        public InspectorWindow() 
        {
            Width = 200;
            Height = 700;
        }

        public override void OnGUI(EditorGUIDrawer drawer)
        {
            EngineEventManager.AddEventListener<OnObjectSelected>(e => DrawInspector(e.SelectedObject, drawer));
            drawer.AddContextItem("Test", (s, a) => Console.WriteLine("yuh"));
        }

        void DrawInspector(GameObject obj, EditorGUIDrawer drawer)
        {
            drawer.Clear();

            drawer.DrawText("Inspector", ElementStyle.DefaultTextStyle.OverrideFontSize(35).OverrideMargin(new System.Windows.Thickness(10)));

            inspector = new Inspector(obj, drawer);

            var (context, button) = drawer.DrawContextButton("Add Component", ElementStyle.DefaultButtonStyle.OverrideMargin(new System.Windows.Thickness(40)).OverridePadding(new System.Windows.Thickness(40, 3, 40, 3)));
           

            foreach(var o in ComponentManager.GetAllComponents())
                context.AddOption("Add "+o.Name, (e, t) =>
                {
                    obj.AddComponent(o);
                    DrawInspector(obj, drawer);
                });
        }

        public override void OnUpdateGUI(EditorGUIDrawer drawer)
        {
            inspector?.Update(drawer);
        }
    }
}
