using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameEngine.Editor.Windows
{
    public partial class EditorGUIDrawer
    {
        public void DrawIcon(string path, ElementStyle style) 
        {
            ImageBrush icon = new ImageBrush();
            icon.ImageSource = new BitmapImage(new Uri(path, UriKind.Relative));

            Border border = new Border();
            ApplyStyle(border, style);

            border.Background = icon;

            Render(border);
        }
    }
}