using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace GameEngine.Editor.Windows
{
    public class StringField : UIComponent
    {
        public void OnDraw(EditorGUIDrawer editorGui, GameEngine.Component owner, FieldInfo field)
        {
            editorGui.StartHorizontalGroup();

            editorGui.DrawText(field.Name);
            editorGui.DrawField((string)field.GetValue(owner.BindingBehavior), (value) =>
            {
                field.SetValue(owner.BindingBehavior, value);
            });

            editorGui.EndGroup();
        }
    }
}
