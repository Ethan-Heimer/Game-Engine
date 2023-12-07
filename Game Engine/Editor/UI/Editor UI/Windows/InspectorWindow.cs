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
            EngineEventManager.AddEventListener<OnObjectSelectedEditorEvent>(e => DrawInspector(e.SelectedObject, drawer));
        }

        void DrawInspector(GameObject obj, EditorGUIDrawer drawer)
        {
            drawer.Clear();

            drawer.DrawText("Inspector", new ElementStyle()
            {
                FontSize = ElementStyle.LargeTextSize
            });
            inspector = new Inspector(obj, drawer);

            drawer.DrawContextMenu();
            
        }

        public override void OnUpdateGUI(EditorGUIDrawer drawer)
        {
            inspector?.Update(drawer);
        }
    }
}
