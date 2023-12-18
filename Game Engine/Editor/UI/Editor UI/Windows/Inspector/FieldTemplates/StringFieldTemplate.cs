using GameEngine.Editor.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows;
using System.Windows.Controls;

namespace GameEngine.Editor.UI.Inspector
{
    [InspectingFieldTemplate]
    internal class StringFieldTemplate : FieldTemplate<string>
    {
        public StringFieldTemplate(Type bindertype, FieldInfo info, object owner) : base(bindertype, info, owner) { }

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
            input.Text = data.GetValue();
        }

        void HandleInput(string value)
        {
            try
            {
                data.SetValue(value);
            }
            catch { }
        }
    }
}
