using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace GameEngine.Editor.Windows
{
    public partial class EditorGUIDrawer
    {
        public Border DrawBox(int width, int height, byte r, byte g, byte b)
        {
            return DrawBox("", new ElementStyle()
            {
                Width = width,
                Height = height,
                Background = ElementStyle.GetColor(r, g, b)
            });
        }

        public Border DrawBox(int width, int height, byte r, byte g, byte b, string tag)
        {
            return DrawBox(tag, new ElementStyle()
            {
                Width = width,
                Height = height,
                Background = ElementStyle.GetColor(r, g, b)
            });
        }

        public Border DrawBox(int width, int height, Brush color)
        {
            return DrawBox("", new ElementStyle()
            {
                Width = width,
                Height = height,
                Background = color
            });
        }

        public Border DrawBox(ElementStyle style)
        {
            return DrawBox("", style);
        }

        public Border DrawBox(string tag, ElementStyle style)
        {
            Border border = new Border();
            border.Name = tag;

            ElementStyle.ApplyStyle(border, style);

            Render(border);

            return border;
        }
    }
}