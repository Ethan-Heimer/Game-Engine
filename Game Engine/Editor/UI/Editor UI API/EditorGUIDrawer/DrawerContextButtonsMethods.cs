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
        public (ContextManager, Button) DrawContextButton(string text)
        {
            return DrawContextButton(text, "", ElementStyle.DefaultButtonStyle);
        }

        public (ContextManager, Button) DrawContextButton(string text, ElementStyle style)
        {
            return DrawContextButton(text, "", style);
        }

        public (ContextManager, Button) DrawContextButton(string text, string tag, ElementStyle style)
        {
            Button button = new Button();
            ApplyStyle(button, style);

            button.Name = tag;
            button.Content = text;

            ContextManager context = new ContextManager(button);

            button.Click += (s, e) => button.ContextMenu.IsOpen = true;

            Render(button);

            return (context, button);
        }
    }
}