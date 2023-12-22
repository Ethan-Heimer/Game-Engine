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
        public Button DrawButton(object content, ButtonClick onClick)
        {
            return DrawButton(content, onClick, "", ElementStyle.DefaultButtonStyle);
        }

        public Button DrawButton(object content, string tag, ButtonClick onClick)
        {
            return DrawButton(content, onClick, tag, ElementStyle.DefaultButtonStyle);
        }

        public Button DrawButton(object content, ButtonClick onClick, ElementStyle style)
        {
            return DrawButton(content, onClick, "", style);
        }

        public Button DrawButton(object content, ButtonClick onClick, string tag, ElementStyle style)
        {
            Button button = new Button();
            ElementStyle.ApplyStyle(button, style);

            button.Name = tag;
            button.Content = content;

            button.Click += (s, e) => onClick.Invoke(s, e);

            Render(button);

            return button;
        }
    }
}
