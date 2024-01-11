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
    public class BoolFieldTemplate : FieldTemplate<bool>
    {
        public BoolFieldTemplate(Type bindertype, FieldInfo info, object owner) : base(bindertype, info, owner){}

        protected override void Template(EditorGUIDrawer drawer, object[] args)
        {
            drawer.DrawText(data.Name, ElementStyle.DefaultTextStyle.OverrideMargin(new Thickness(10)));
            drawer.DrawCheckBox(data.GetValue(), HandleInput, "box", ElementStyle.DefaultCheckboxStyle);
        }


        protected override void OnValueChanged()
        {
            var checkBox = FindElementInTemplate<CheckBox>("box");
            checkBox.IsChecked = data.GetValue();
        }

        protected void HandleInput(bool val) => data.SetValue(val);
       

    }
}
