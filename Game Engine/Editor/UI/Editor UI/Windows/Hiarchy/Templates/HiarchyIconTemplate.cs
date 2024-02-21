using GameEngine.Debugging;
using GameEngine.Editor.UI.Inspector;
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

namespace GameEngine.Editor.UI.Hiarchy
{
    [HiarchyFieldTemplate]
    public class HiarchyIconTemplate : FieldTemplate<Icon>
    {
        public HiarchyIconTemplate(Type bindertype, MemberValue info, object owner) : base(bindertype, info, owner)
        {
            templateStyle = new ElementStyle()
            {
                DynamicSize = true,
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

        }
        protected override void Template(EditorGUIDrawer drawer, object[] args) 
        {
            int padding = args.Length > 0 ? (int)args[0] | 0 : 0;

            if (((GameObject)owner).Parent != null)
                drawer.DrawIcon("child icon", new ElementStyle()
                {
                    Width = 15,
                    Height = 15,
                    Margin = new Thickness(padding * 20, 0, 0, 0)
                });
            drawer.DrawIcon(((GameObject)owner).Icon, "icon", new ElementStyle()
            {
                Width = 30,
                Height = 30
            });
        }

        protected override void OnValueChanged()
        {
            FindElementInTemplate<Border>("icon").Background = ElementStyle.GetImage(data.GetValue());
        }
    }
}
