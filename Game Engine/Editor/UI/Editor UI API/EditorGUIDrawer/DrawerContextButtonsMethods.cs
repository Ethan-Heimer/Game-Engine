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
        public (ContextManager, Button) DrawContextButton(object content)
        {
            return DrawContextButton(content, "", ElementStyle.DefaultButtonStyle);
        }

        public (ContextManager, Button) DrawContextButton(object content, ElementStyle style)
        {
            return DrawContextButton(content, "", style);
        }

        public (ContextManager, Button) DrawContextButton(object content, string tag, ElementStyle style)
        {
            Button button = new Button();
            ElementStyle.ApplyStyle(button, style);

            button.Name = tag;
            button.Content = content;

            ContextManager context = new ContextManager(button);

            button.Click += (s, e) => button.ContextMenu.IsOpen = true;

            Render(button);

            return (context, button);
        }
    }
}