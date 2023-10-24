using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GameEngine.Editor.Windows
{
    public class EditorGUIDrawer
    {
        private StackPanel content;
        public EditorGUIDrawer(StackPanel _content) 
        {
           content = _content;
        }

        public void Label(string text)
        {
            TextBlock textBlock = new TextBlock()
            {
                Text = text
            };

            textBlock.Style = (Style)Application.Current.Resources["Label"];
            content.Children.Add(textBlock);
        }
    }
}
