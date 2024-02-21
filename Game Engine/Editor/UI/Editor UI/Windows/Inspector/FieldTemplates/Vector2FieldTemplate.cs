using GameEngine.Editor.Windows;
using GameEngine.Engine.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GameEngine.Editor.UI.Inspector
{
    [InspectingFieldTemplate]
    public class Vector2FieldTemplate : FieldTemplate<Vector2>
    {
        public Vector2FieldTemplate(Type bindertype, MemberValue info, object owner) : base(bindertype, info, owner){}

        protected override void Template(EditorGUIDrawer drawer, object[] args)
        {
            drawer.DrawText(data.Name, ElementStyle.DefaultTextStyle.OverrideMargin(new Thickness(10)));

            var fieldX =  drawer.DrawField((val) =>
            {
                HandleInputX(val);
            }, "X", ElementStyle.DefaultFieldStyle.OverrideBorderBrush(Brushes.Red));

            var fieldY = drawer.DrawField((val) =>
            {
                HandleInputY(val);
            }, "Y", ElementStyle.DefaultFieldStyle.OverrideBorderBrush(Brushes.LawnGreen));
        }


        protected override void OnValueChanged()
        {
            var x = FindElementInTemplate<TextBox>("X");
            var y = FindElementInTemplate<TextBox>("Y");

            x.Text = data.GetValue().X.ToString();
            y.Text = data.GetValue().Y.ToString();
        }

        void HandleInputX(string value)
        {
            try
            {
                float number = float.Parse(value);
                data.SetValue(new Vector2(number, data.GetValue().Y));
            }
            catch { }
        }

        void HandleInputY(string value)
        {
            try
            {
                float number = float.Parse(value);
                data.SetValue(new Vector2(data.GetValue().X, number));
            }
            catch { }
        }

    }
}
