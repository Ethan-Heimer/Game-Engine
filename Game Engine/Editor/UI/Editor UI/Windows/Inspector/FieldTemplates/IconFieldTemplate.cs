using GameEngine.Editor.Windows;
using GameEngine.Engine;
using GameEngine.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GameEngine.Editor.UI.Inspector
{
    [InspectingFieldTemplate]
    public class IconFieldTemplate : FieldTemplate<Icon>
    {
        public IconFieldTemplate(Type bindertype, MemberValue info, object owner) : base(bindertype, info, owner) { }

        protected override void Template(EditorGUIDrawer drawer, object[] args)
        {
            drawer.DrawText(data.Name, ElementStyle.DefaultTextStyle.OverrideMargin(new Thickness(10)));

            var icon = drawer.GetIcon(((GameObject)owner).Icon, new ElementStyle()
            {
                Width = 25,
                Height = 25,
            });

            var (menu, button) =drawer.DrawContextButton(icon, "button", new ElementStyle() 
            {
                Width = 25,
                Height = 25,
            });

            string[] Icons = AssetManager.GetIconNames();

            foreach (var o in Icons) 
            {
                menu.AddOption(o, o, (s, e) =>
                {
                    AssetManager.GetIcon(o, out Icon newIcon);
                    data.SetValue(newIcon);
                    icon.Background = ElementStyle.GetImage(newIcon);
                });
            }
        }

    }
}