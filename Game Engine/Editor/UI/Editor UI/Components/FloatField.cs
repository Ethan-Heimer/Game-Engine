using GameEngine.Editor.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.Windows
{
    public class FloatField : Component
    {
        public void OnDraw(EditorGUIDrawer editorGui, GameEngine.Component owner, FieldInfo field)
        {
            editorGui.StartHorizontalGroup();

            editorGui.DrawText(field.Name);
            editorGui.DrawField(field.GetValue(owner.BindingBehavior).ToString(), (value) =>
            {
                Console.WriteLine(value.ToString());
                if (value == "")
                    return;

                field.SetValue(owner.BindingBehavior, float.Parse(value));
            });

            editorGui.EndGroup();
        }
    }
}
