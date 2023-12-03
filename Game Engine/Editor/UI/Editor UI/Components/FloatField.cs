using GameEngine.Editor.UI.Inspector;
using GameEngine.Editor.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.Windows
{
    public class FloatField : UIComponent
    {
        public void OnDraw(EditorGUIDrawer editorGui, Type BinderType, FieldInfo field, object owner)
        {
            IFieldBinder<float> data = (IFieldBinder<float>)Activator.CreateInstance(BinderType, new object[] { field, owner });

            editorGui.StartHorizontalGroup();

            editorGui.DrawText(data.Name);

            editorGui.DrawField(data.GetValue().ToString(), (value) =>
            {
                Console.WriteLine(value.ToString());
                if (value == "")
                    return;

                data.SetValue(float.Parse(value));
            });

            editorGui.EndGroup();
        }
    }
}
