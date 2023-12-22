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
        public TextBlock DrawText(string text)
        {
            return DrawText(text, "", ElementStyle.DefaultTextStyle);
        }

        public TextBlock DrawText(string text, string tag)
        {
            return DrawText(text, tag, ElementStyle.DefaultTextStyle);
        }

        public TextBlock DrawText(string text, ElementStyle style)
        {
            return DrawText(text, "", style);
        }
        public TextBlock DrawText(string text, string tag, ElementStyle style)
        {
            TextBlock textBlock = new TextBlock();
            ElementStyle.ApplyStyle(textBlock, style);

            textBlock.Name = tag;
            textBlock.Text = text;

            Render(textBlock);

            return textBlock;
        }
    }
}