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
    public class HiarchyActiveTemplate : FieldTemplate<bool>
    {
        public HiarchyActiveTemplate(Type bindertype, MemberValue info, object owner) : base(bindertype, info, owner)
        {
            templateStyle = new ElementStyle()
            {
                DynamicSize = true,
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

        }
        protected override void Template(EditorGUIDrawer drawer, object[] args) 
        {
            Icon icon;
            AssetManager.GetIcon("Active", out icon);

            drawer.DrawIcon(icon, "icon", new ElementStyle()
            {
                Width = 30,
                Height = 30
            });
        }

        protected override void OnValueChanged()
        {
            Icon icon;
            if (data.GetValue())
                AssetManager.GetIcon("Active", out icon);
            else
                AssetManager.GetIcon("Inactive", out icon);

            FindElementInTemplate<Border>("icon").Background = ElementStyle.GetImage(icon);
        }
    }
}
