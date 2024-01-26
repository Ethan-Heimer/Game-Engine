using GameEngine.Debugging;
using GameEngine.Editor.UI.Inspector;
using GameEngine.Editor.Windows;
using GameEngine.Engine.Utilities;
using Microsoft.Build.Tasks.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameEngine.Editor.UI.Hiarchy
{
    [HiarchyFieldTemplate]
    [Note(note ="Make Icon Manager")]
    public class HiarchyNameTemplate : FieldTemplate<string>
    {
        public HiarchyNameTemplate(Type bindertype, MemberValue info, object owner) : base(bindertype, info, owner) 
        {
            templateStyle = new ElementStyle()
            {
                DynamicSize = true,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                DragEffects = true,
                OnDragBackground = ElementStyle.TertiaryBackgroundColor
            };

        }

        protected override void Template(EditorGUIDrawer drawer, object[] args)
        {
            EnableDragDrop(owner);

            template.MouseDown += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    EditorEventManager.SelectedObject = (GameObject)owner;
                }
            };

            drawer.DrawText(data.GetValue(), "name", new ElementStyle()
            {
                BorderBrush = ElementStyle.PrimaryBackgroundColor,
                DynamicSize = true,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 3, 0, 3)
            });
        }

        protected override void OnValueChanged()
        {
           FindElementInTemplate<TextBlock>("name").Text = data.GetValue();
        }

        protected override void OnDragLeave(EditorGUIDrawer drawer, object data)
        {
            var gameObject = (GameObject)data;

            gameObject.Parent?.RemoveChild(gameObject);

        }

        protected override void OnDrop(EditorGUIDrawer drawer, object data)
        {
            var child = (GameObject)data;

            if (owner == child || ((GameObject)owner).Parent == child)
                return;

            child.Parent?.RemoveChild(child);
            ((GameObject)owner).AddChild(child);
        }
    }
}
