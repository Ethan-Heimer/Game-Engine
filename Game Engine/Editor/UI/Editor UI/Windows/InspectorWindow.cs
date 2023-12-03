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

namespace GameEngine.Editor.Windows
{
    [OpenWindowByDefault]
    [Note(note = "there Needs to be a way to tell when the ui needs to be rerendered")]
    public class InspectorWindow : EditorWindow
    {
        

        GameObject target;
        int refreshRate = 1000;

        public InspectorWindow() 
        {
            Width = 200;
            Height = 700;
        }

        public override void OnGUI(EditorGUIDrawer drawer)
        {
            drawer.DrawLabel("Inspector");
            EngineEventManager.AddEventListener<OnObjectSelectedEditorEvent>(e => SetTarget(e.SelectedObject, drawer));
            //ontargetdeselected
            //EngineEventManager.AddEventListener<WhileInEditMode>(e => RenderObjectData(drawer));
        }

        void SetTarget(GameObject obj, EditorGUIDrawer drawer)
        {
            target = obj;
            Inspector inspector = new Inspector(target, drawer);
        }

        void RenderObjectData(EditorGUIDrawer drawer)
        {
            if (target == null)
                return;

            drawer.Clear();

            drawer.DrawLabel("Inspector");
            drawer.DrawText(target.ToString());

            
        }

        void UpdateInspector()
        {

        }
    }
}
