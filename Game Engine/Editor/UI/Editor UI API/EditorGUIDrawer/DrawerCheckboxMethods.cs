using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GameEngine.Editor.Windows
{
    public partial class EditorGUIDrawer
    {
        public CheckBox DrawCheckBox(Action<bool> onChanged)
        {
            return DrawCheckBox(onChanged, ElementStyle.DefaultCheckboxStyle);
        }

        public CheckBox DrawCheckBox(Action<bool> onChanged, ElementStyle style)
        {
            return DrawCheckBox(onChanged, "", style);
        }

        public CheckBox DrawCheckBox(Action<bool> onChanged, string tag, ElementStyle elementStyle)
        {
            return DrawCheckBox(false, onChanged, tag, elementStyle);
        }

        public CheckBox DrawCheckBox(bool defaultValue, Action<bool> onChanged, string tag, ElementStyle elementStyle)
        {
            CheckBox checkBox = new CheckBox();
            ElementStyle.ApplyStyle(checkBox, elementStyle);

            checkBox.Name = tag;
            checkBox.IsChecked = defaultValue;

            checkBox.Checked += (e, s) => onChanged((bool)checkBox.IsChecked);
            checkBox.Unchecked += (e, s) => onChanged((bool)checkBox.IsChecked);

            Render(checkBox);

            return checkBox;
        }
    }
}