using GameEngine.Editor.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows;
using Microsoft.Xna.Framework;
using System.Windows.Media;
using System.Windows.Controls;
using GameEngine.Rendering;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Engine.Utilities;

namespace GameEngine.Editor.UI.Inspector
{
    [InspectingFieldTemplate]
    public class SpriteFieldTemplate : FieldTemplate<Sprite>
    {
        public SpriteFieldTemplate(Type bindertype, MemberValue info, object owner) : base(bindertype, info, owner) { }

        protected override void Template(EditorGUIDrawer drawer, object[] args)
        {
            drawer.DrawText(data.Name, ElementStyle.DefaultTextStyle.OverrideMargin(new Thickness(10)));

            var display = drawer.DrawField(SetSprite, "field", ElementStyle.DefaultFieldStyle.OverrideWidth(100));
            drawer.FileSelector("+", "png", OnFileSelected, new ElementStyle()
            {
                Width = 20,
                Height = 20,
                Background = ElementStyle.TertiaryBackgroundColor,
                BorderBrush = Brushes.Black
            });

        }

        protected override void OnValueChanged()
        {
            var display = FindElementInTemplate<TextBox>("field");
            display.Text = data.GetValue()?.Path;
        }

        void OnFileSelected(string path) 
        {
            string fileName = Path.GetFileNameWithoutExtension(path);

            var display = FindElementInTemplate<TextBox>("field");
            display.Text = fileName;
            SetSprite(fileName);
        }

        void SetSprite(string input)
        {
            data.SetValue(new Sprite(input));
        }

       
    }
}
