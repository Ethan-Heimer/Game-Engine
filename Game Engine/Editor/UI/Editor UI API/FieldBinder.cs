using GameEngine.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameEngine.Editor.UI.Inspector
{
    [Note(note ="Every frame, GetValue() is called AT LEAST once (in HasValueChanged) as well as when ever Get Value is called. while maybe not that big of a performance hit, it is somthing to look into")]
    public class ComponentFieldBinder<T> : IFieldBinder<T> 
    {
        public FieldInfo Field { get; set; }
        public object Owner { get; set; }

        T Value;
        public ComponentFieldBinder(FieldInfo field, object owner)
        {
            Field = field;
            Owner = owner;

            Value = GetValue();
        }

        public string Name
        {
            get
            {
                return Field.Name;
            }
        }

        public T GetValue()
        {
            return (T)Field.GetValue(Owner);
        }

        public void SetValue(T value)
        {
            Value = value;
            Field.SetValue(Owner, value);
        }

        public bool HasValueChange()
        {
            T val = GetValue();

            if (!Equals(Value, val))
            {
                Value = val;
                return true;
            }

            return false;
        }
    }
}
