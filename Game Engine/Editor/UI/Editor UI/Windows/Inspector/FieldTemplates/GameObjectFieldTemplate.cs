using GameEngine.Editor.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows;
using Microsoft.Xna.Framework;
using System.Windows.Media;
using System.Windows.Controls;
using GameEngine.Rendering;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Engine.Utilities;

namespace GameEngine.Editor.UI.Inspector
{
    [InspectingFieldTemplate]
    public class GameObjectFieldTemplate : FieldTemplate<GameObject>
    {
        public GameObjectFieldTemplate(Type bindertype, MemberValue info, object owner) : base(bindertype, info, owner) 
        {
            
        }

        protected override void Template(EditorGUIDrawer drawer, object[] args)
        {
            drawer.DrawText(data.Name, ElementStyle.DefaultTextStyle.OverrideMargin(new Thickness(10)));

            ElementStyle style = new ElementStyle()
            {
                Background = ElementStyle.AccentBackgroundColor,
                BorderBrush = ElementStyle.TertiaryBackgroundColor,
            };

            string text = data.GetValue() == null ? "" : data.GetValue().Name;

            drawer.DrawText(text, "ref", style);

        }

        protected override void OnValueChanged()
        {

            var field = FindElementInTemplate<TextBlock>("ref");
            string text = data.GetValue() == null ? "" : data.GetValue().Name;
            field.Text = text;
        }

        protected override void OnMouseEnter(EditorGUIDrawer drawer)
        {
            if (EditorEventManager.CapturedGameObject == null)
                return;

            var field = FindElementInTemplate<TextBlock>("ref");
            field.Background = ElementStyle.TertiaryBackgroundColor;
        }

        protected override void OnMouseExit(EditorGUIDrawer drawer)
        {
            if (EditorEventManager.CapturedGameObject == null)
                return;

            var field = FindElementInTemplate<TextBlock>("ref");
            field.Background = ElementStyle.AccentBackgroundColor;
        }

        protected override void OnMouseUp(EditorGUIDrawer drawer)
        {
            GameObject obj = EditorEventManager.CapturedGameObject;

            if(obj != null)
            {
                data.SetValue(obj);
                OnValueChanged();

                var field = FindElementInTemplate<TextBlock>("ref");
                field.Background = ElementStyle.AccentBackgroundColor;
            }

        }

    }
}
