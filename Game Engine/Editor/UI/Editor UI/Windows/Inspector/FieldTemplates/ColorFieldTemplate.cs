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

namespace GameEngine.Editor.UI.Inspector
{
    [InspectingFieldTemplate]
    public class ColorFieldTemplate : FieldTemplate<Microsoft.Xna.Framework.Color>
    {
        public ColorFieldTemplate(Type bindertype, FieldInfo info, object owner) : base(bindertype, info, owner) { }

        Vector3 color;

        protected override void Template(EditorGUIDrawer drawer)
        {
            var colorData = data.GetValue();
            color = new Vector3(colorData.R/255f, colorData.G/255f, colorData.B/255f);

            drawer.DrawText(data.Name, ElementStyle.DefaultTextStyle.OverrideMargin(new Thickness(10)));

            drawer.StartVerticalGroup();
            drawer.StartHorizontalGroup();

            var fieldR = drawer.DrawField(data.GetValue().R.ToString(), (val) =>
            {
                HandleInput(val, ref color.X);
                Console.WriteLine(val.ToString());
            }, "R", ElementStyle.DefaultFieldStyle.OverrideBorderBrush(Brushes.Red));

            var fieldG = drawer.DrawField(data.GetValue().G.ToString(), (val) =>
            {
                HandleInput(val, ref color.Y);
                Console.WriteLine(val.ToString());
            }, "G", ElementStyle.DefaultFieldStyle.OverrideBorderBrush(Brushes.LawnGreen));

            var fieldB = drawer.DrawField(data.GetValue().B.ToString(), (val) =>
            {
                HandleInput(val, ref color.Z);
                Console.WriteLine(val.ToString());
            }, "B", ElementStyle.DefaultFieldStyle.OverrideBorderBrush(Brushes.DeepSkyBlue));

            drawer.EndGroup();

            drawer.DrawBox("Color", new ElementStyle()
            {
                Width = 340,
                Height = 25,
                Margin = new Thickness(10),
                Background = new SolidColorBrush(new System.Windows.Media.Color()
                {
                    R = (byte)(color.X * 255),
                    G = (byte)(color.Y * 255),
                    B = (byte)(color.Z * 255),
                    A = data.GetValue().A
                })
            });

            drawer.EndGroup();
        }


        protected override void OnValueChanged()
        {
            var r = FindElementInTemplate<TextBox>("R");
            var g = FindElementInTemplate<TextBox>("G");
            var b = FindElementInTemplate<TextBox>("B");

            r.Text = data.GetValue().R.ToString();
            g.Text = data.GetValue().G.ToString();
            b.Text = data.GetValue().B.ToString();

            SetColorDisplay(color);

            Console.WriteLine(data.GetValue() + " data");

        }

        void HandleInput(string value, ref float output)
        {
            try
            {
                float number = float.Parse(value);
                output = number/255f;

                UpdateColor();
                Console.WriteLine(color);
                Console.WriteLine(new Microsoft.Xna.Framework.Color(color));
            }
            catch { }
        }

        void UpdateColor()
        {
            data.SetValue(new Microsoft.Xna.Framework.Color(color)); //colors need to within 0-1
            SetColorDisplay(color);
        }

        void SetColorDisplay(Vector3 color)
        {
            var colorDisplay = FindElementInTemplate<Border>("Color");
            colorDisplay.Background = new SolidColorBrush(new System.Windows.Media.Color()
            {
                R = (byte)(color.X * 255),
                G = (byte)(color.Y * 255),
                B = (byte)(color.Z * 255),
                A = data.GetValue().A
            });
        }

    }
}
