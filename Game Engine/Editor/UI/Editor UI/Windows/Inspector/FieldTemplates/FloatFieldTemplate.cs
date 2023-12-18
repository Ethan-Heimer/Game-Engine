using GameEngine.Editor.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GameEngine.Editor.UI.Inspector
{
    [InspectingFieldTemplate]
    public class FloatFieldTemplate : FieldTemplate<float>
    {
        public FloatFieldTemplate(Type bindertype, FieldInfo info, object owner) : base(bindertype, info, owner){}

        protected override void Template(EditorGUIDrawer drawer)
        {
            drawer.DrawText(data.Name, ElementStyle.DefaultTextStyle.OverrideMargin(new Thickness(10)));

            var field = drawer.DrawField((val) =>
            {
               HandleInput(val);
            }, "field", ElementStyle.DefaultFieldStyle);
           
        }


        protected override void OnValueChanged()
        {
            var input = FindElementInTemplate<TextBox>("field");
            input.Text = data.GetValue().ToString();
        }

        void HandleInput(string value)
        {
            try
            {
                float number = float.Parse(value);
                data.SetValue(number);
            }
            catch { }
        }

    }
}
