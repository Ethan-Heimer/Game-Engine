﻿using GameEngine.Engine.Events;
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
using System.Runtime.Serialization;
using Microsoft.Build.Tasks.Xaml;
using GameEngine.Engine.ComponentModel;
using System.Windows.Media;
using System.Windows.Controls;
using GameEngine.ComponentManagement;
using GameEngine.Engine.Rendering;

namespace GameEngine.Editor.Windows
{
    [OpenWindowByDefault]
    public class InspectorWindow : EditorWindow
    {
        Inspector inspector;
        public InspectorWindow() 
        {
            Width = 300;
            Height = 500;

            RelativePosition = RelativeWindowPosition.Right;
        }

        public override void OnGUI(EditorGUIDrawer drawer)
        {
            EngineEventManager.AddEventListener<OnObjectSelected>(e => DrawInspector(e.SelectedObject, drawer));

            EngineEventManager.AddEventListener<OnComponentAdded>(e => DrawInspector(EditorEventManager.SelectedObject, drawer));
            EngineEventManager.AddEventListener<OnComponentRemoved>(e => DrawInspector(EditorEventManager.SelectedObject, drawer));
        }

        void DrawInspector(GameObject obj, EditorGUIDrawer drawer)
        {
            drawer.Clear();
            
            if (obj == null)
                return;

            drawer.DrawText("Inspector", ElementStyle.DefaultTextStyle.OverrideFontSize(35).OverrideMargin(new System.Windows.Thickness(10)));

            inspector = new Inspector(obj, drawer);

            var (context, button) = drawer.DrawContextButton("Add Component", ElementStyle.DefaultButtonStyle.OverrideMargin(new System.Windows.Thickness(40)).OverridePadding(new System.Windows.Thickness(40, 3, 40, 3)));
           
            foreach(var o in ComponentManager.GetAllComponents())
                context.AddOption("Add "+o.Name, (e, t) =>
                {
                    obj.AddComponent(o);
                });
        }

        public override void OnUpdateGUI(EditorGUIDrawer drawer)
        {
            inspector?.Update(drawer);
        }
    }
}
