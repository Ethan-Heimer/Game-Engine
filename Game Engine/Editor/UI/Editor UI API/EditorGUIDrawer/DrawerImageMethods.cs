using GameEngine.Engine;
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

        public Border GetIcon(Icon icon, ElementStyle style)
        {
            return GetIcon(icon, "", style);
        }

        public Border GetIcon(Icon icon, string tag, ElementStyle style)
        {
            Border border = new Border();
            ElementStyle.ApplyStyle(border, style);

            if (icon?.GetImage() != null)
                border.Background = ElementStyle.GetImage(icon);

            border.Name = tag;

            return border;
        }

        public Border GetIcon(string path, ElementStyle style)
        {
            ImageBrush iconBrush = new ImageBrush();

            if (AssetManager.GetIcon(path, out Icon icon))
                iconBrush.ImageSource = icon.GetImage();
            
            Border border = new Border();
            ElementStyle.ApplyStyle(border, style);

            border.Background = iconBrush;
            return border;  
        }

        public void DrawIcon(Icon icon, ElementStyle style)
        {
            DrawIcon(icon, "", style);
        }

        public void DrawIcon(Icon icon, string tag, ElementStyle style)
        {
            var element = GetIcon(icon, tag, style);
            Render(element);
        }

        public void DrawIcon(string path, ElementStyle style) 
        {
            var element = GetIcon(path, style);

            Render(element);
        }
    }
}