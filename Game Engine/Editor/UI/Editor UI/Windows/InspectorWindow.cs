using GameEngine.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using GameEngine.Debugging;
using System.Reflection;

namespace GameEngine.Editor.Windows
{
    [OpenWindowByDefault]
    [Note(note = "Data Binder for inspector")]
    public class InspectorWindow : EditorWindow
    {
        public InspectorWindow() 
        {
            Width = 200;
            Height = 700;
        }

        public override void OnGUI(EditorGUIDrawer drawer)
        {
            drawer.DrawLabel("Inspector");
            EngineEventManager.AddEventListener<OnObjectSelectedEditorEvent>(e => RenderObjectData(e.SelectedObject, drawer));
        }

        void RenderObjectData(GameObject obj, EditorGUIDrawer drawer)
        {
            drawer.Clear();

            drawer.DrawLabel("Inspector");
            drawer.DrawText(obj.ToString());

            foreach(GameEngine.Component o in obj.GetAllComponents())
            {
                drawer.DrawText(o.BindingBehavior.GetType().Name);
                foreach(FieldInfo f in o.GetFields())
                {
                    if (f.FieldType == typeof(string))
                        drawer.Draw<StringField>(o, f);
                    if (f.FieldType == typeof(float))
                        drawer.Draw<FloatField>(o, f);
                    if (f.FieldType == typeof(int))
                        drawer.Draw<IntField>(o, f);
                }
            }
        }
    }
}
