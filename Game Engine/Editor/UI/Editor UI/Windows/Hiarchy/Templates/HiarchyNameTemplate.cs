using GameEngine.Debugging;
using GameEngine.Editor.UI.Inspector;
using GameEngine.Editor.Windows;
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
        public HiarchyNameTemplate(Type bindertype, FieldInfo info, object owner) : base(bindertype, info, owner) 
        {
            templateStyle = new ElementStyle()
            {
                DynamicSize = true,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = ElementStyle.PrimaryBackgroundColor
            };

        }

        protected override void Template(EditorGUIDrawer drawer)
        {
            EnableDragDrop(owner);

            template.MouseDown += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    EditorEventManager.SelectedObject = (GameObject)owner;
                }
            };


            if (((GameObject)owner).parent != null)
                drawer.DrawIcon("..\\..\\editor\\UI\\Editor UI\\Icons\\child icon.png", new ElementStyle()
                {
                    Width = 15,
                    Height = 15
                });

            drawer.DrawIcon("..\\..\\editor\\UI\\Editor UI\\Icons\\Cube.Png", new ElementStyle()
            {
                Width = 30,
                Height = 30
            });

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

        protected override void OnMouseEnter(EditorGUIDrawer drawer)
        {
            template.Background = ElementStyle.AccentBackgroundColor;
        }

        protected override void OnMouseExit(EditorGUIDrawer drawer)
        {
            template.Background = ElementStyle.PrimaryBackgroundColor;
        }

        protected override void OnDragEnter(EditorGUIDrawer drawer, object data)
        {
            template.Background = ElementStyle.TertiaryBackgroundColor;
        }

        protected override void OnDragLeave(EditorGUIDrawer drawer, object data)
        {
            var gameObject = (GameObject)data;

            gameObject.parent?.RemoveChild(gameObject);

            template.Background = ElementStyle.PrimaryBackgroundColor;
        }

        protected override void OnDrop(EditorGUIDrawer drawer, object data)
        {
            var child = (GameObject)data;

            if (owner == child || ((GameObject)owner).parent == child)
                return;

            child.parent?.RemoveChild(child);
            ((GameObject)owner).AddChild(child);

            template.Background = ElementStyle.PrimaryBackgroundColor;
        }
    }
}
