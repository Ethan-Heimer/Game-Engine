using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.Windows
{
    public class IntField: UIComponent
    {
        public void OnDraw(EditorGUIDrawer editorGui, GameEngine.Component owner, FieldInfo field)
        {
            editorGui.StartHorizontalGroup();

            editorGui.DrawText(field.Name);
            editorGui.DrawField(field.GetValue(owner.BindingBehavior).ToString(), (value) =>
            {
                if (value != null)
                    return;

                field.SetValue(owner.BindingBehavior, int.Parse(value));
            });

            editorGui.EndGroup();
        }
    }
}
