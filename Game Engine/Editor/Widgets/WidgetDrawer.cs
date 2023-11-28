using GameEngine.Engine;
using GameEngine.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace GameEngine.Editor.Widgets
{
    public static class WidgetDrawer
    {
        static List<Widget> widgets = new List<Widget>();

        public static void Init()
        {
            EngineEventManager.AddEventListener<WhileInEditMode>(e => DrawPersistent());
        }

        public static void Draw(Widget widget)
        {
            widget.GetType().GetMethod("OnDraw", BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance).Invoke(widget, new object[0]);
        }

        public static void Draw(Widget widget, bool enabled)
        {
            if (enabled)
                Draw(widget);
            else
                DrawDisabled(widget);
        }

        public static void AddStaticWidget(Widget widget)
        {
            widgets.Add(widget);
        }

        public static void RemoveStaticWidget(Widget widget)
        {
            widgets.Remove(widget);
        }

        static void DrawPersistent()
        {
            foreach(Widget widget in widgets) 
            {
                Draw(widget);
            }
        }

        static void DrawDisabled(Widget widget)
        {
            widget.GetType().GetMethod("OnDisabled", BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance).Invoke(widget, new object[0]);
        }
    }
}
