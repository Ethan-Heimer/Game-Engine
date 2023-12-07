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
    public class Vector2FieldTemplate : FieldTemplate<Vector2>
    {
        public Vector2FieldTemplate(Type bindertype, FieldInfo info, object owner) : base(bindertype, info, owner){}

        protected override FrameworkElement Template(EditorGUIDrawer drawer)
        {
            drawer.StartHorizontalGroup();

            drawer.DrawText(data.Name, ElementStyle.DefaultTextStyle().OverrideMargin(new Thickness(10)));

            var fieldX =  drawer.DrawField((val) =>
            {
                HandleInputX(val);
                Console.WriteLine(val.ToString());
            }, "X", ElementStyle.DefaultFieldStyle().OverrideBorderColor(Brushes.Red));

            var fieldY = drawer.DrawField((val) =>
            {
                HandleInputY(val);
                Console.WriteLine(val.ToString());
            }, "Y", ElementStyle.DefaultFieldStyle().OverrideBorderColor(Brushes.LawnGreen));

            drawer.EndGroup();

            return drawer.GetContainer();
        }


        protected override void OnValueChanged(EditorGUIDrawer drawer)
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
