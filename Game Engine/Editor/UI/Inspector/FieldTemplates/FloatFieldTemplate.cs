using GameEngine.Editor.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.UI.Inspector
{
    public class FloatFieldTemplate : IFieldTemplate<float>
    {
        public UIComponent GetUI(FieldInfo info)
        {
            FloatField field = new FloatField();
            return field;
        }
    }
}
