using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GameEngine.Editor.Windows
{
    public partial class EditorGUIDrawer
    {
        public TextBox GetField(Action<string> onValueChanged)
        {
            return GetField("", onValueChanged);
        }
        public TextBox GetField(string defaultText, Action<string> onValueChanged)
        {
            return GetField(defaultText, onValueChanged, "", ElementStyle.DefaultButtonStyle);
        }
        public TextBox GetField(string defaultText, Action<string> onValueChanged, ElementStyle style)
        {
            return GetField(defaultText, onValueChanged, "", style);
        }
        public TextBox GetField(string defaultText, Action<string> onValueChanged, string tag, ElementStyle style)
        {
            TextBox textBox = new TextBox();
            ApplyStyle(textBox, style);

            textBox.Name = tag;
            textBox.Text = defaultText;

            textBox.TextChanged += (s, e) => { onValueChanged(textBox.Text); };

            return textBox;
        }

        public TextBox DrawField(Action<string> onValueChanged)
        {
            return DrawField("", onValueChanged, "", new ElementStyle());
        }

        public TextBox DrawField(string defaultText, Action<string> onValueChanged)
        {
            return DrawField(defaultText, onValueChanged, "", new ElementStyle());
        }

        public TextBox DrawField(Action<string> onValueChanged, ElementStyle style)
        {
            return DrawField("", onValueChanged, "", style);
        }

        public TextBox DrawField(Action<string> onValueChanged, string tag, ElementStyle style)
        {
            return DrawField("", onValueChanged, tag, style);
        }

        public TextBox DrawField(string defaultText, Action<string> onValueChanged, string tag,  ElementStyle style)
        {
            var field = GetField(defaultText, onValueChanged, tag, style);
            Render(field); 
            return field;
        }
    }
}